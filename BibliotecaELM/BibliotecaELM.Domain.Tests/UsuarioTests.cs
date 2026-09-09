using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using Xunit;

namespace BibliotecaELM.Domain.Tests;

/// <summary>
/// Testes unitários de domínio para a entidade Usuario.
/// Valida invariantes e regras de negócio sem mocks.
/// </summary>
public class UsuarioTests
{
    // ──────────────────────────── Caminho Feliz ────────────────────────────

    [Fact]
    public void CriarUsuario_ComDadosValidos_DeveInstanciarComSucesso()
    {
        // Arrange
        var nome = "Ada Lovelace";
        var nascimento = new DateOnly(1815, 12, 10);
        var email = "ada@pioneers.org";
        var cpf = "12345678901";

        // Act
        var usuario = new Usuario(nome, nascimento, email, cpf, null);

        // Assert
        Assert.NotNull(usuario);
        Assert.Equal(nome, usuario.NomeUsuario);
        Assert.Equal(nascimento, usuario.Nascimento);
        Assert.Equal(email, usuario.Email);
        Assert.Equal(cpf, usuario.Cpf);
        Assert.Null(usuario.Endereco);
    }

    // ──────────────────────────── Caminhos de Erro ────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarUsuario_ComNomeInvalido_DeveLancarBusinessRuleValidationException(string? nomeInvalido)
    {
        // Arrange
        var nascimento = new DateOnly(1990, 1, 1);
        var email = "usuario@teste.com";
        var cpf = "12345678901";

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Usuario(nomeInvalido!, nascimento, email, cpf, null);
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarUsuario_ComEmailInvalido_DeveLancarBusinessRuleValidationException(string? emailInvalido)
    {
        // Arrange
        var nome = "Alan Turing";
        var nascimento = new DateOnly(1912, 6, 23);
        var cpf = "12345678901";

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Usuario(nome, nascimento, emailInvalido!, cpf, null);
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarUsuario_ComCpfInvalido_DeveLancarBusinessRuleValidationException(string? cpfInvalido)
    {
        // Arrange
        var nome = "Grace Hopper";
        var nascimento = new DateOnly(1906, 12, 9);
        var email = "grace@navy.mil";

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Usuario(nome, nascimento, email, cpfInvalido!, null);
        });
    }
}
