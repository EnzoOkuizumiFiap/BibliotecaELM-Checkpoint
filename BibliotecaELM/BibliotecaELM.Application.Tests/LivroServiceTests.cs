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
}
