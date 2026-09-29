namespace BibliotecaELM.Application.DTOs;

/// <summary>
/// Parâmetros de paginação por offset (page / pageSize).
/// Padrão: page=1, pageSize=20 (máximo 100).
/// </summary>
public class PaginationQuery
{
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;

    /// <summary>Número da página (1-based).</summary>
    public int Page { get; set; } = 1;

    /// <summary>Quantidade de itens por página (máximo 100).</summary>
    public int PageSize { get; set; } = DefaultPageSize;

    /// <summary>Valida os limites de page e pageSize.</summary>
    public bool TryGetError(out string? message)
    {
        if (Page < 1)
        {
            message = "O parâmetro 'page' deve ser maior ou igual a 1.";
            return true;
        }

        if (PageSize is < 1 or > MaxPageSize)
        {
            message = $"O parâmetro 'pageSize' deve estar entre 1 e {MaxPageSize}.";
            return true;
        }

        message = null;
        return false;
    }
}
