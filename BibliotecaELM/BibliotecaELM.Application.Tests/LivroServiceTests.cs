using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Implementations;
using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using Moq;
using Xunit;

namespace BibliotecaELM.Application.Tests;

/// <summary>
/// Testes de Application para LivroService.
/// Valida orquestração do serviço com mocks de repositório (ILivroRepository + IAutorRepository).
/// </summary>
public class LivroServiceTests
{
    private readonly Mock<ILivroRepository> _livroRepositoryMock;
    private readonly Mock<IAutorRepository> _autorRepositoryMock;
    private readonly LivroService _service;

    public LivroServiceTests()
    {
        _livroRepositoryMock = new Mock<ILivroRepository>();
        _autorRepositoryMock = new Mock<IAutorRepository>();
        _service = new LivroService(_livroRepositoryMock.Object, _autorRepositoryMock.Object);
    }

    // ──────────────── Cenário: dependência ausente → não persiste (Times.Never) ────────────────

    /// <summary>
    /// Quando o AutorId informado não existe no repositório de Autores,
    /// deve lançar ResourceNotFoundException e NÃO chamar Add (Times.Never).
    /// </summary>
    [Fact]
    public void Create_QuandoAutorNaoExiste_DeveLancarResourceNotFoundExceptionENaoChamarAdd()
    {
        // Arrange
        var autorIdInexistente = Guid.NewGuid();
        var request = new LivroRequest(
            "Clean Code",
            150.00m,
            new DateOnly(2008, 8, 1),
            autorIdInexistente
        );

        _livroRepositoryMock
            .Setup(r => r.ExistsByNomeLivro(request.NomeLivro))
            .Returns(false);

        _autorRepositoryMock
            .Setup(r => r.ExistsById(autorIdInexistente))
            .Returns(false); // Autor NÃO existe → dispara ResourceNotFoundException

        // Act & Assert
        Assert.Throws<ResourceNotFoundException>(() => _service.Create(request));

        // Repositório de livro NUNCA deve ser chamado para persistir
        _livroRepositoryMock.Verify(r => r.Add(It.IsAny<Livro>()), Times.Never);
    }

    // ──────────────── Cenário: caminho feliz → persiste exatamente uma vez (Times.Once) ────────────────

    /// <summary>
    /// Quando todos os dados são válidos e o Autor existe,
    /// deve retornar LivroResponse e chamar Add exatamente uma vez (Times.Once).
    /// </summary>
    [Fact]
    public void Create_QuandoDadosValidosEAutorExiste_DeveChamarAddUmaVez()
    {
        // Arrange
        var autorIdValido = Guid.NewGuid();
        var request = new LivroRequest(
            "Domain-Driven Design",
            200.00m,
            new DateOnly(2003, 8, 22),
            autorIdValido
        );

        _livroRepositoryMock
            .Setup(r => r.ExistsByNomeLivro(request.NomeLivro))
            .Returns(false);

        _autorRepositoryMock
            .Setup(r => r.ExistsById(autorIdValido))
            .Returns(true); // Autor EXISTE → fluxo continua

        // Act
        var result = _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.NomeLivro, result.NomeLivro);
        _livroRepositoryMock.Verify(r => r.Add(It.IsAny<Livro>()), Times.Once);
    }

    // ──────────────── CP5: Paginação — Parâmetros Inválidos (Theory) ────────────────

    /// <summary>
    /// Quando page ou pageSize estão fora da faixa permitida,
    /// deve lançar BusinessRuleValidationException e NÃO consultar o repositório.
    /// </summary>
    [Theory]
    [InlineData(0, 20, "O parâmetro 'page' deve ser maior ou igual a 1.")]
    [InlineData(-1, 20, "O parâmetro 'page' deve ser maior ou igual a 1.")]
    [InlineData(1, 0, "O parâmetro 'pageSize' deve estar entre 1 e 100.")]
    [InlineData(1, -5, "O parâmetro 'pageSize' deve estar entre 1 e 100.")]
    [InlineData(1, 101, "O parâmetro 'pageSize' deve estar entre 1 e 100.")]
    [InlineData(1, 9999, "O parâmetro 'pageSize' deve estar entre 1 e 100.")]
    public void GetPaged_QuandoParametrosInvalidos_DeveLancarBusinessRuleValidationException(
        int page, int pageSize, string mensagemEsperada)
    {
        // Act & Assert
        var exception = Assert.Throws<BusinessRuleValidationException>(() => _service.GetPaged(page, pageSize));
        Assert.Equal(mensagemEsperada, exception.Message);

        // Repositório NÃO deve ser consultado quando a validação falhar
        _livroRepositoryMock.Verify(
            r => r.GetPaged(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Func<IQueryable<Livro>, IOrderedQueryable<Livro>>>()),
            Times.Never);
    }

    // ──────────────── CP5: Paginação — Caminho Feliz (Fact) ────────────────

    /// <summary>
    /// Quando page e pageSize são válidos, deve retornar o envelope paginado correto
    /// com totalPages, hasPrevious e hasNext calculados.
    /// </summary>
    [Fact]
    public void GetPaged_QuandoParametrosValidos_DeveRetornarEnvelopePaginadoCorreto()
    {
        // Arrange
        var autorId = Guid.NewGuid();
        var livros = new List<Livro>
        {
            new Livro("Livro A", 50m, new DateOnly(2020, 1, 1), autorId),
            new Livro("Livro B", 70m, new DateOnly(2021, 1, 1), autorId)
        };

        _livroRepositoryMock
            .Setup(r => r.GetPaged(
                1, 2,
                It.IsAny<Func<IQueryable<Livro>, IOrderedQueryable<Livro>>>()))
            .Returns((livros.AsReadOnly(), 5)); // 5 itens no total, 2 na página 1

        // Act
        var result = _service.GetPaged(1, 2);

        // Assert — envelope correto
        Assert.NotNull(result);
        Assert.Equal(1, result.Page);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(5, result.TotalItems);
        Assert.Equal(3, result.TotalPages); // ceil(5 / 2) = 3
        Assert.False(result.HasPrevious);   // primeira página
        Assert.True(result.HasNext);        // há mais páginas
        Assert.Equal(2, result.Items.Count);
    }
}
