using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Enums;
using BibliotecaELM.Domain.Exceptions;
using Xunit;

namespace BibliotecaELM.Domain.Tests;

/// <summary>
/// Testes unitários de domínio para a entidade Compra.
/// Valida regras de negócio de data, usuário obrigatório e lista de livros sem mocks.
/// </summary>
public class CompraTests
{
    private static Livro CriarLivroMockado()
    {
        return new Livro("Livro Teste", 50m, new DateOnly(2020, 1, 1), Guid.NewGuid());
    }

    // ──────────────────────────── Caminho Feliz ────────────────────────────

    [Fact]
    public void CriarCompra_ComDadosValidos_DeveInstanciarComSucesso()
    {
        // Arrange
        var formaCompra = FormaCompraEnum.Crédito;
        var dataCompra = DateTime.Now.AddMinutes(-5);
        var usuarioId = Guid.NewGuid();
        var livros = new List<Livro> { CriarLivroMockado() };

        // Act
        var compra = new Compra(formaCompra, dataCompra, usuarioId, livros);

        // Assert
        Assert.NotNull(compra);
        Assert.Equal(formaCompra, compra.FormaCompra);
        Assert.Equal(usuarioId, compra.UsuarioId);
        Assert.Single(compra.Livros);
    }

    // ──────────────────────────── Caminhos de Erro ────────────────────────────

    [Fact]
    public void CriarCompra_ComDataNoFuturo_DeveLancarBusinessRuleValidationException()
    {
        // Arrange
        var formaCompra = FormaCompraEnum.Pix;
        var dataFutura = DateTime.Now.AddDays(1);
        var usuarioId = Guid.NewGuid();
        var livros = new List<Livro> { CriarLivroMockado() };

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Compra(formaCompra, dataFutura, usuarioId, livros);
        });
    }

    [Fact]
    public void CriarCompra_ComUsuarioIdVazio_DeveLancarBusinessRuleValidationException()
    {
        // Arrange
        var formaCompra = FormaCompraEnum.Dinheiro;
        var dataCompra = DateTime.Now.AddHours(-1);
        var usuarioIdVazio = Guid.Empty;
        var livros = new List<Livro> { CriarLivroMockado() };

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Compra(formaCompra, dataCompra, usuarioIdVazio, livros);
        });
    }

    [Fact]
    public void CriarCompra_SemLivros_DeveLancarBusinessRuleValidationException()
    {
        // Arrange
        var formaCompra = FormaCompraEnum.Pix;
        var dataCompra = DateTime.Now.AddMinutes(-10);
        var usuarioId = Guid.NewGuid();
        var listaVazia = new List<Livro>();

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Compra(formaCompra, dataCompra, usuarioId, listaVazia);
        });
    }
}
