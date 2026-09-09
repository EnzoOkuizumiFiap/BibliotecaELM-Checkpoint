using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Implementations;
using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using Moq;
using Xunit;

namespace BibliotecaELM.Application.Tests;

/// <summary>
/// Testes de Application para UsuarioService.
/// Valida regras de negócio de aplicação com mock de IUsuarioRepository (Times.Never e Times.Once).
/// </summary>
public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _service = new UsuarioService(_usuarioRepositoryMock.Object);
    }

    [Fact]
    public void Create_QuandoEmailJaExiste_DeveLancarBusinessRuleValidationExceptionENaoChamarAdd()
    {
        // Arrange
        var request = new UsuarioRequest("Linus Torvalds", new DateOnly(1969, 12, 28), "linus@kernel.org", "12345678901", null);

        _usuarioRepositoryMock
            .Setup(r => r.ExistsByEmail(request.Email))
            .Returns(true); // E-mail já cadastrado

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => _service.Create(request));

        _usuarioRepositoryMock.Verify(r => r.Add(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public void Create_QuandoDadosValidos_DeveRetornarUsuarioResponseEChamarAddUmaVez()
    {
        // Arrange
        var request = new UsuarioRequest("Dennis Ritchie", new DateOnly(1941, 9, 9), "dmr@bell-labs.com", "12345678901", null);

        _usuarioRepositoryMock
            .Setup(r => r.ExistsByEmail(request.Email))
            .Returns(false);

        // Act
        var result = _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.NomeUsuario, result.NomeUsuario);
        Assert.Equal(request.Email, result.Email);
        _usuarioRepositoryMock.Verify(r => r.Add(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact]
    public void Delete_QuandoUsuarioNaoExiste_DeveRetornarFalseENaoChamarDelete()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();

        _usuarioRepositoryMock
            .Setup(r => r.GetById(idInexistente))
            .Returns((Usuario?)null);

        // Act
        var result = _service.Delete(idInexistente);

        // Assert
        Assert.False(result);
        _usuarioRepositoryMock.Verify(r => r.Delete(It.IsAny<Usuario>()), Times.Never);
    }
}
