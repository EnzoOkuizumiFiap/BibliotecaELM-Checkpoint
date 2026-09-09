using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Implementations;
using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Enums;
using BibliotecaELM.Domain.Exceptions;
using Moq;
using Xunit;

namespace BibliotecaELM.Application.Tests;

/// <summary>
/// Testes de Application para CompraService.
/// Valida orquestração de múltiplos mocks (ICompraRepository, IUsuarioRepository, IEnderecoRepository, ILivroRepository).
/// </summary>
public class CompraServiceTests
{
    private readonly Mock<ICompraRepository> _compraRepositoryMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IEnderecoRepository> _enderecoRepositoryMock;
    private readonly Mock<ILivroRepository> _livroRepositoryMock;
    private readonly CompraService _service;

    public CompraServiceTests()
    {
        _compraRepositoryMock = new Mock<ICompraRepository>();
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _enderecoRepositoryMock = new Mock<IEnderecoRepository>();
        _livroRepositoryMock = new Mock<ILivroRepository>();

        _service = new CompraService(
            _compraRepositoryMock.Object,
            _usuarioRepositoryMock.Object,
            _enderecoRepositoryMock.Object,
            _livroRepositoryMock.Object);
    }

    // ──────────────── Cenário 1: Usuário inexistente → não persiste (Times.Never) ────────────────

    [Fact]
    public void Create_QuandoUsuarioNaoExiste_DeveLancarResourceNotFoundExceptionENaoChamarAdd()
    {
        // Arrange
        var usuarioIdInexistente = Guid.NewGuid();
        var livroId = Guid.NewGuid();
        var request = new CompraRequest(
            FormaCompraEnum.Crédito,
            DateTime.Now.AddMinutes(-5),
            usuarioIdInexistente,
            new List<Guid> { livroId });

        _usuarioRepositoryMock
            .Setup(r => r.ExistsById(usuarioIdInexistente))
            .Returns(false); // Usuário NÃO existe

        // Act & Assert
        Assert.Throws<ResourceNotFoundException>(() => _service.Create(request));

        _compraRepositoryMock.Verify(r => r.Add(It.IsAny<Compra>()), Times.Never);
    }

    // ──────────────── Cenário 2: Usuário sem endereço → não persiste (Times.Never) ────────────────

    [Fact]
    public void Create_QuandoUsuarioNaoPossuiEndereco_DeveLancarBusinessRuleValidationExceptionENaoChamarAdd()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var livroId = Guid.NewGuid();
        var request = new CompraRequest(
            FormaCompraEnum.Pix,
            DateTime.Now.AddMinutes(-5),
            usuarioId,
            new List<Guid> { livroId });

        _usuarioRepositoryMock
            .Setup(r => r.ExistsById(usuarioId))
            .Returns(true); // Usuário existe

        _enderecoRepositoryMock
            .Setup(r => r.ExistsByIdUsuario(usuarioId))
            .Returns(false); // Mas NÃO tem endereço cadastrado!

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => _service.Create(request));

        _compraRepositoryMock.Verify(r => r.Add(It.IsAny<Compra>()), Times.Never);
    }

    // ──────────────── Cenário 3: Caminho feliz → persiste exatamente uma vez (Times.Once) ────────────────

    [Fact]
    public void Create_QuandoDadosValidosEUsuarioComEndereco_DeveChamarAddUmaVezERetornarResponse()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var livroId = Guid.NewGuid();
        var livro = new Livro("Livro Válido", 100m, new DateOnly(2021, 1, 1), Guid.NewGuid());

        var request = new CompraRequest(
            FormaCompraEnum.Pix,
            DateTime.Now.AddMinutes(-5),
            usuarioId,
            new List<Guid> { livroId });

        _usuarioRepositoryMock
            .Setup(r => r.ExistsById(usuarioId))
            .Returns(true);

        _enderecoRepositoryMock
            .Setup(r => r.ExistsByIdUsuario(usuarioId))
            .Returns(true);

        _livroRepositoryMock
            .Setup(r => r.GetById(livroId))
            .Returns(livro);

        // Act
        var result = _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(usuarioId, result.UsuarioId);
        _compraRepositoryMock.Verify(r => r.Add(It.IsAny<Compra>()), Times.Once);
    }
}
