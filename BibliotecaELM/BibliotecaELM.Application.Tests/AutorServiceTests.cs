using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Implementations;
using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using Moq;
using Xunit;

namespace BibliotecaELM.Application.Tests;

/// <summary>
/// Testes de Application para AutorService.
/// Valida regras de orquestração com mock de IAutorRepository (Times.Never e Times.Once).
/// </summary>
public class AutorServiceTests
{
    private readonly Mock<IAutorRepository> _autorRepositoryMock;
    private readonly AutorService _service;

    public AutorServiceTests()
    {
        _autorRepositoryMock = new Mock<IAutorRepository>();
        _service = new AutorService(_autorRepositoryMock.Object);
    }

    // ──────────────── Cenário: nome duplicado → não persiste (Times.Never) ────────────────

    [Fact]
    public void Create_QuandoNomeAutorJaExiste_DeveLancarBusinessRuleValidationExceptionENaoChamarAdd()
    {
        // Arrange
        var request = new AutorRequest("Machado de Assis", new DateOnly(1839, 6, 21));

        _autorRepositoryMock
            .Setup(r => r.ExistsByNomeAutor(request.NomeAutor))
            .Returns(true); // Nome já existe!

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => _service.Create(request));

        _autorRepositoryMock.Verify(r => r.Add(It.IsAny<Autor>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_QuandoNomeAutorVazio_DeveLancarBusinessRuleValidationExceptionENaoChamarAdd(string nomeVazio)
    {
        // Arrange
        var request = new AutorRequest(nomeVazio, new DateOnly(1920, 1, 1));

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => _service.Create(request));

        _autorRepositoryMock.Verify(r => r.Add(It.IsAny<Autor>()), Times.Never);
    }

    // ──────────────── Cenário: caminho feliz → persiste uma vez (Times.Once) ────────────────

    [Fact]
    public void Create_QuandoDadosValidos_DeveChamarAddUmaVezERetornarResponse()
    {
        // Arrange
        var request = new AutorRequest("Clarice Lispector", new DateOnly(1920, 12, 10));

        _autorRepositoryMock
            .Setup(r => r.ExistsByNomeAutor(request.NomeAutor))
            .Returns(false);

        // Act
        var result = _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.NomeAutor, result.NomeAutor);
        _autorRepositoryMock.Verify(r => r.Add(It.IsAny<Autor>()), Times.Once);
    }

    // ──────────────── Cenário: recurso ausente na atualização / exclusão ────────────────

    [Fact]
    public void Update_QuandoAutorNaoExiste_DeveRetornarNullENaoChamarUpdate()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();
        var request = new AutorRequest("Autor Fantasma", new DateOnly(1950, 1, 1));

        _autorRepositoryMock
            .Setup(r => r.GetById(idInexistente))
            .Returns((Autor?)null);

        // Act
        var result = _service.Update(idInexistente, request);

        // Assert
        Assert.Null(result);
        _autorRepositoryMock.Verify(r => r.Update(It.IsAny<Autor>()), Times.Never);
    }

    [Fact]
    public void Delete_QuandoAutorNaoExiste_DeveRetornarFalseENaoChamarDelete()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();

        _autorRepositoryMock
            .Setup(r => r.GetById(idInexistente))
            .Returns((Autor?)null);

        // Act
        var result = _service.Delete(idInexistente);

        // Assert
        Assert.False(result);
        _autorRepositoryMock.Verify(r => r.Delete(It.IsAny<Autor>()), Times.Never);
    }

    [Fact]
    public void Delete_QuandoAutorExiste_DeveChamarDeleteUmaVezERetornarTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var autor = new Autor("Guimarães Rosa", new DateOnly(1908, 6, 27), new List<Livro>());

        _autorRepositoryMock
            .Setup(r => r.GetById(id))
            .Returns(autor);

        // Act
        var result = _service.Delete(id);

        // Assert
        Assert.True(result);
        _autorRepositoryMock.Verify(r => r.Delete(autor), Times.Once);
    }
}
