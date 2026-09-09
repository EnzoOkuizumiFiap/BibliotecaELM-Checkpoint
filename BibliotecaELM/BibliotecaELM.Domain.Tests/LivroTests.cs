using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using Xunit;

namespace BibliotecaELM.Domain.Tests;

public class LivroTests
{
    // ──────────────────────────── Caminho Feliz ────────────────────────────

    /// <summary>
    /// [Fact] - AAA explícito: cria um livro com dados válidos e verifica as propriedades.
    /// </summary>
    [Fact]
    public void CriarLivro_ComDadosValidos_DeveInstanciarComSucesso()
    {
        // Arrange
        var nomeLivro = "Clean Code";
        var preco = 200m;
        var dataLancamento = new DateOnly(2000, 1, 1);
        var autorId = Guid.NewGuid();

        // Act
        var livro = new Livro(nomeLivro, preco, dataLancamento, autorId);

        // Assert
        Assert.NotNull(livro);
        Assert.Equal(nomeLivro, livro.NomeLivro);
        Assert.Equal(preco, livro.Preco);
        Assert.Equal(dataLancamento, livro.DataLancamento);
        Assert.Equal(autorId, livro.AutorId);
    }

    // ──────────────────────────── Caminho de Erro ──────────────────────────

    /// <summary>
    /// [Theory] - Regra de domínio: nome do livro é obrigatório.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarLivro_ComNomeLivroInvalido_DeveLancarBusinessRuleValidationException(string? nomeLivroInvalido)
    {
        // Arrange
        var preco = 200m;
        var dataLancamento = new DateOnly(2000, 1, 1);
        var autorId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Livro(nomeLivroInvalido!, preco, dataLancamento, autorId);
        });
    }

    /// <summary>
    /// [Theory] - Regra de domínio: preço não pode ser negativo.
    /// </summary>
    [Theory]
    [InlineData(-0.01)]
    [InlineData(-100)]
    [InlineData(-9999.99)]
    public void CriarLivro_ComPrecoNegativo_DeveLancarBusinessRuleValidationException(decimal precoInvalido)
    {
        // Arrange
        var nomeLivro = "Domain-Driven Design";
        var dataLancamento = new DateOnly(2003, 8, 22);
        var autorId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Livro(nomeLivro, precoInvalido, dataLancamento, autorId);
        });
    }

    /// <summary>
    /// [Fact] - Regra de domínio: AutorId não pode ser Guid.Empty.
    /// </summary>
    [Fact]
    public void CriarLivro_ComAutorIdVazio_DeveLancarBusinessRuleValidationException()
    {
        // Arrange
        var nomeLivro = "The Pragmatic Programmer";
        var preco = 150m;
        var dataLancamento = new DateOnly(1999, 10, 30);
        var autorIdVazio = Guid.Empty;

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Livro(nomeLivro, preco, dataLancamento, autorIdVazio);
        });
    }
}