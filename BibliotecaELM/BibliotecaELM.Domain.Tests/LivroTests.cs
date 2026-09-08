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
        var titulo = "Clean Code";
        var isbn = "9780132350884";
        var anoPublicacao = 2008;

        // Act
        var livro = new Livro(titulo, isbn, anoPublicacao);

        // Assert
        Assert.NotNull(livro);
        Assert.Equal(titulo, livro.Titulo);
        Assert.Equal(isbn, livro.Isbn);
        Assert.Equal(anoPublicacao, livro.AnoPublicacao);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarLivro_ComTituloInvalido_DeveLancarDomainException(string tituloInvalido)
    {
        // Arrange
        var isbn = "9780132350884";
        var anoPublicacao = 2008;

        // Act & Assert
        Assert.Throws<DomainException>(() => new Livro(tituloInvalido, isbn, anoPublicacao));
    }
}