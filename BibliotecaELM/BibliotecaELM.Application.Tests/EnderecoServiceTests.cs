using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Implementations;
using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using Moq;
using Xunit;

namespace BibliotecaELM.Application.Tests;

/// <summary>
/// Testes de Application para EnderecoService.
/// Valida orquestração de mocks (IEnderecoRepository, IUsuarioRepository) com Times.Never e Times.Once.
/// </summary>
public class EnderecoServiceTests
{
    private readonly Mock<IEnderecoRepository> _enderecoRepositoryMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly EnderecoService _service;

    public EnderecoServiceTests()
    {
        _enderecoRepositoryMock = new Mock<IEnderecoRepository>();
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();

        _service = new EnderecoService(
            _enderecoRepositoryMock.Object,
            _usuarioRepositoryMock.Object);
    }

    [Fact]
    public void Create_QuandoUsuarioNaoExiste_DeveLancarResourceNotFoundExceptionENaoChamarAdd()
    {
        // Arrange
        var usuarioIdInexistente = Guid.NewGuid();
        var request = new EnderecoRequest("01311-000", "SP", "São Paulo", "Bela Vista", "Av. Paulista");

        _usuarioRepositoryMock
            .Setup(r => r.ExistsById(usuarioIdInexistente))
            .Returns(false); // Usuário não existe!

        // Act & Assert
        Assert.Throws<ResourceNotFoundException>(() => _service.Create(request, usuarioIdInexistente));

        _enderecoRepositoryMock.Verify(r => r.Add(It.IsAny<Endereco>()), Times.Never);
    }

    [Fact]
    public void Create_QuandoUsuarioJaPossuiEndereco_DeveLancarBusinessRuleValidationExceptionENaoChamarAdd()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var request = new EnderecoRequest("01311-000", "SP", "São Paulo", "Bela Vista", "Av. Paulista");

        _usuarioRepositoryMock
            .Setup(r => r.ExistsById(usuarioId))
            .Returns(true); // Usuário existe

        _enderecoRepositoryMock
            .Setup(r => r.ExistsByIdUsuario(usuarioId))
            .Returns(true); // Mas já tem endereço cadastrado!

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => _service.Create(request, usuarioId));

        _enderecoRepositoryMock.Verify(r => r.Add(It.IsAny<Endereco>()), Times.Never);
    }

    [Fact]
    public void Create_QuandoDadosValidosEUsuarioSemEndereco_DeveChamarAddUmaVezERetornarResponse()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var request = new EnderecoRequest("01311-000", "SP", "São Paulo", "Bela Vista", "Av. Paulista");

        _usuarioRepositoryMock
            .Setup(r => r.ExistsById(usuarioId))
            .Returns(true);

        _enderecoRepositoryMock
            .Setup(r => r.ExistsByIdUsuario(usuarioId))
            .Returns(false);

        // Act
        var result = _service.Create(request, usuarioId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Cep, result.Cep);
        _enderecoRepositoryMock.Verify(r => r.Add(It.IsAny<Endereco>()), Times.Once);
    }

    [Fact]
    public void Delete_QuandoEnderecoNaoExiste_DeveRetornarFalseENaoChamarDelete()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();

        _enderecoRepositoryMock
            .Setup(r => r.GetById(idInexistente))
            .Returns((Endereco?)null);

        // Act
        var result = _service.Delete(idInexistente);

        // Assert
        Assert.False(result);
        _enderecoRepositoryMock.Verify(r => r.Delete(It.IsAny<Endereco>()), Times.Never);
    }
}
