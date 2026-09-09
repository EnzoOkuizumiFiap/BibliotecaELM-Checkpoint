using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using Xunit;

namespace BibliotecaELM.Domain.Tests;

/// <summary>
/// Testes unitários de domínio para a entidade Emprestimo.
/// Valida regras de negócio de data de empréstimo, data de devolução, usuário obrigatório e lista de livros sem mocks.
/// </summary>
public class EmprestimoTests
{
    private static Livro CriarLivroMockado()
    {
        return new Livro("Estruturas de Dados", 120m, new DateOnly(2018, 5, 10), Guid.NewGuid());
    }

    // ──────────────────────────── Caminho Feliz ────────────────────────────

    [Fact]
    public void CriarEmprestimo_ComDadosValidos_DeveInstanciarComSucesso()
    {
        // Arrange
        var dataEmprestimo = DateTime.Now.AddDays(-7);
        var dataDevolucao = DateTime.Now.AddDays(-1);
        var usuarioId = Guid.NewGuid();
        var livros = new List<Livro> { CriarLivroMockado() };

        // Act
        var emprestimo = new Emprestimo(dataEmprestimo, dataDevolucao, usuarioId, livros);

        // Assert
        Assert.NotNull(emprestimo);
        Assert.Equal(dataEmprestimo, emprestimo.DataEmprestimo);
        Assert.Equal(dataDevolucao, emprestimo.DataDevolucao);
        Assert.Equal(usuarioId, emprestimo.UsuarioId);
        Assert.Single(emprestimo.Livros);
    }

    // ──────────────────────────── Caminhos de Erro ────────────────────────────

    [Fact]
    public void CriarEmprestimo_ComDataEmprestimoNoFuturo_DeveLancarBusinessRuleValidationException()
    {
        // Arrange
        var dataFutura = DateTime.Now.AddDays(2);
        var dataDevolucao = DateTime.Now.AddDays(10);
        var usuarioId = Guid.NewGuid();
        var livros = new List<Livro> { CriarLivroMockado() };

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Emprestimo(dataFutura, dataDevolucao, usuarioId, livros);
        });
    }

    [Fact]
    public void CriarEmprestimo_ComDataDevolucaoAnteriorADataEmprestimo_DeveLancarBusinessRuleValidationException()
    {
        // Arrange
        var dataEmprestimo = DateTime.Now.AddDays(-3);
        var dataDevolucaoAnterior = DateTime.Now.AddDays(-5); // anterior ao empréstimo
        var usuarioId = Guid.NewGuid();
        var livros = new List<Livro> { CriarLivroMockado() };

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Emprestimo(dataEmprestimo, dataDevolucaoAnterior, usuarioId, livros);
        });
    }

    [Fact]
    public void CriarEmprestimo_ComUsuarioIdVazio_DeveLancarBusinessRuleValidationException()
    {
        // Arrange
        var dataEmprestimo = DateTime.Now.AddDays(-2);
        var dataDevolucao = DateTime.Now.AddDays(-1);
        var usuarioIdVazio = Guid.Empty;
        var livros = new List<Livro> { CriarLivroMockado() };

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Emprestimo(dataEmprestimo, dataDevolucao, usuarioIdVazio, livros);
        });
    }

    [Fact]
    public void CriarEmprestimo_SemLivros_DeveLancarBusinessRuleValidationException()
    {
        // Arrange
        var dataEmprestimo = DateTime.Now.AddDays(-2);
        var dataDevolucao = DateTime.Now.AddDays(-1);
        var usuarioId = Guid.NewGuid();
        var listaVazia = new List<Livro>();

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Emprestimo(dataEmprestimo, dataDevolucao, usuarioId, listaVazia);
        });
    }
}
