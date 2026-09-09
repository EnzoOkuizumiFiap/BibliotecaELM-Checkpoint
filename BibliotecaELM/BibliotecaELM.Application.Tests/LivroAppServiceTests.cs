using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Implementations;
using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
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
    public async Task CriarLivroAsync_QuandoAutorIdNulo_DeveLancarArgumentExceptionENaoChamarAdd()
    {
        var request = new LivroRequest(
            "Clean Code",
            150.00m,
            new DateOnly(2008, 8, 1),
            null
        );
        var traceId = "test-trace-id";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarLivroAsync(request, traceId));

        _repositoryMock.Verify(r => r.Add(It.IsAny<Livro>()), Times.Never);
    }

    [Fact]
    public async Task CriarLivroAsync_QuandoDadosValidos_DeveChamarAddUmaVez()
    {
        var request = new LivroRequest(
            "Domain-Driven Design",
            200.00m,
            new DateOnly(2003, 8, 22),
            Guid.NewGuid()
        );
        var traceId = "test-trace-id";

        var result = await _service.CriarLivroAsync(request, traceId);

        Assert.NotNull(result);
        _repositoryMock.Verify(r => r.Add(It.IsAny<Livro>()), Times.Once);
    }
}