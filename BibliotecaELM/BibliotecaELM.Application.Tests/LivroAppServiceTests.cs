using BibliotecaELM.Application.Services;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BibliotecaELM.Application.Tests;

public class LivroAppServiceTests
{
    private readonly Mock<ILivroRepository> _repositoryMock;
    private readonly Mock<ILogger<LivroAppService>> _loggerMock;
    private readonly LivroAppService _service;

    public LivroAppServiceTests()
    {
        _repositoryMock = new Mock<ILivroRepository>();
        _loggerMock = new Mock<ILogger<LivroAppService>>();
        _service = new LivroAppService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CriarLivro_QuandoTituloInvalido_NaoDeveChamarAdicionarNoRepositorio()
    {
        // Arrange
        var command = new CriarLivroCommand("", "9780132350884", 2008);
        var traceId = "test-trace-id";

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _service.CriarLivroAsync(command, traceId));

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Livro>()), Times.Never);
    }

    [Fact]
    public async Task CriarLivro_QuandoDadosValidos_DeveChamarAdicionarUmaVez()
    {
        // Arrange
        var command = new CriarLivroCommand("Domain-Driven Design", "9780321125217", 2003);
        var traceId = "test-trace-id";

        // Act
        var result = await _service.CriarLivroAsync(command, traceId);

        // Assert
        Assert.NotNull(result);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Livro>()), Times.Once);
    }
}