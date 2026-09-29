using BibliotecaELM.Application.DTOs;
using Xunit;

namespace BibliotecaELM.Application.Tests;

/// <summary>
/// Testes unitários para validação de limites de PaginationQuery.
/// </summary>
public class PaginationQueryTests
{
    [Fact]
    public void PaginationQuery_ValoresPadrao_DevemSerValidos()
    {
        var query = new PaginationQuery();

        Assert.Equal(1, query.Page);
        Assert.Equal(20, query.PageSize);
        Assert.False(query.TryGetError(out var error));
        Assert.Null(error);
    }

    [Theory]
    [InlineData(0, "O parâmetro 'page' deve ser maior ou igual a 1.")]
    [InlineData(-1, "O parâmetro 'page' deve ser maior ou igual a 1.")]
    [InlineData(-100, "O parâmetro 'page' deve ser maior ou igual a 1.")]
    public void PaginationQuery_QuandoPageInvalido_DeveRetornarErro(int page, string mensagemEsperada)
    {
        var query = new PaginationQuery { Page = page, PageSize = 20 };

        var hasError = query.TryGetError(out var message);

        Assert.True(hasError);
        Assert.Equal(mensagemEsperada, message);
    }

    [Theory]
    [InlineData(0, "O parâmetro 'pageSize' deve estar entre 1 e 100.")]
    [InlineData(-1, "O parâmetro 'pageSize' deve estar entre 1 e 100.")]
    [InlineData(101, "O parâmetro 'pageSize' deve estar entre 1 e 100.")]
    [InlineData(9999, "O parâmetro 'pageSize' deve estar entre 1 e 100.")]
    public void PaginationQuery_QuandoPageSizeInvalido_DeveRetornarErro(int pageSize, string mensagemEsperada)
    {
        var query = new PaginationQuery { Page = 1, PageSize = pageSize };

        var hasError = query.TryGetError(out var message);

        Assert.True(hasError);
        Assert.Equal(mensagemEsperada, message);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(5, 50)]
    [InlineData(10, 100)]
    public void PaginationQuery_QuandoValoresValidos_NaoDeveRetornarErro(int page, int pageSize)
    {
        var query = new PaginationQuery { Page = page, PageSize = pageSize };

        var hasError = query.TryGetError(out var message);

        Assert.False(hasError);
        Assert.Null(message);
    }
}
