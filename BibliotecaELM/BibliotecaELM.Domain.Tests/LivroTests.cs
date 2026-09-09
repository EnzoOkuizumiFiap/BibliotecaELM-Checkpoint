using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using Xunit;

namespace BibliotecaELM.Domain.Tests;

public class LivroTests
{
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
        // Espera a exceção exata lançada pela validação de domínio
        Assert.Throws<BusinessRuleValidationException>(() =>
        {
            _ = new Livro(nomeLivroInvalido!, preco, dataLancamento, autorId);
        });
    }
}