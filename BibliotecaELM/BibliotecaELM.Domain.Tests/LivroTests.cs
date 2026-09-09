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
        var preco = 200;
        var dataLancamento = "2000-01-01";
        var autorId = 2;

        // Act
        var livro = new Livro(nomeLivro, preco, dataLancamento, autorId);

        // Assert
        Assert.NotNull(livro);
        Assert.Equal(nomeLivro, livro.nomeLivro);
        Assert.Equal(preco, livro.preco);
        Assert.Equal(dataLancamento, livro.dataLancamento);
        Assert.Equal(autorId, livro.autorId)
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarLivro_ComnomeLivroInvalido_DeveLancarDomainException(string nomeLivroInvalido)
    {
        // Arrange
        var preco = 200;
        var dataLancamento = "2000-01-01";
        var autorId = 2;

        // Act & Assert
        Assert.Throws<DomainException>(() => new Livro(nomeLivroInvalido, preco, dataLancamento, autorId));
    }
}