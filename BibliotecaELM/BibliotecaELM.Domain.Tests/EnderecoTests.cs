using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using Xunit;

namespace BibliotecaELM.Domain.Tests;

/// <summary>
/// Testes unitários de domínio para a entidade Endereco.
/// Valida campos obrigatórios (CEP, Estado, Cidade, Bairro, Rua, UsuarioId) sem mocks.
/// </summary>
public class EnderecoTests
{
    // ──────────────────────────── Caminho Feliz ────────────────────────────

    [Fact]
    public void CriarEndereco_ComDadosValidos_DeveInstanciarComSucesso()
    {
        // Arrange
        var cep = "01311-000";
        var estado = "SP";
        var cidade = "São Paulo";
        var bairro = "Bela Vista";
        var rua = "Av. Paulista";
        var usuarioId = Guid.NewGuid();

        // Act
        var endereco = new Endereco(cep, estado, cidade, bairro, rua, usuarioId);

        // Assert
        Assert.NotNull(endereco);
        Assert.Equal(cep, endereco.Cep);
        Assert.Equal(estado, endereco.Estado);
        Assert.Equal(cidade, endereco.Cidade);
        Assert.Equal(bairro, endereco.Bairro);
        Assert.Equal(rua, endereco.Rua);
        Assert.Equal(usuarioId, endereco.UsuarioId);
    }

    // ──────────────────────────── Caminhos de Erro ────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarEndereco_ComCepInvalido_DeveLancarBusinessRuleValidationException(string? cepInvalido)
    {
        // Arrange
        var usuarioId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Endereco(cepInvalido!, "SP", "São Paulo", "Centro", "Rua A", usuarioId);
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarEndereco_ComEstadoInvalido_DeveLancarBusinessRuleValidationException(string? estadoInvalido)
    {
        // Arrange
        var usuarioId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Endereco("01311-000", estadoInvalido!, "São Paulo", "Centro", "Rua A", usuarioId);
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarEndereco_ComCidadeInvalida_DeveLancarBusinessRuleValidationException(string? cidadeInvalida)
    {
        // Arrange
        var usuarioId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Endereco("01311-000", "SP", cidadeInvalida!, "Centro", "Rua A", usuarioId);
        });
    }

    [Fact]
    public void CriarEndereco_ComUsuarioIdVazio_DeveLancarBusinessRuleValidationException()
    {
        // Arrange
        var usuarioIdVazio = Guid.Empty;

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Endereco("01311-000", "SP", "São Paulo", "Centro", "Rua A", usuarioIdVazio);
        });
    }
}
