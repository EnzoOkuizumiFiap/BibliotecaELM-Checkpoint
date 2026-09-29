namespace BibliotecaELM.Application.DTOs;

/// <summary>
/// Envelope padrão de listagens paginadas (v2).
/// Segue o padrão de PagedResponse do Recommenda com totais e indicadores de navegação.
/// </summary>
public record PagedResponse<T>(
    int Page,
    int PageSize,
    int TotalItems,
    IReadOnlyList<T> Items)
{
    /// <summary>Total de páginas calculadas com base no pageSize atual.</summary>
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalItems / (double)PageSize);

    /// <summary>Indica se existe página anterior.</summary>
    public bool HasPrevious => Page > 1;

    /// <summary>Indica se existe próxima página.</summary>
    public bool HasNext => Page < TotalPages;

    /// <summary>
    /// Construtor alternativo no formato (items, page, pageSize, totalItems) para conveniência e paridade com Recommenda.
    /// </summary>
    public PagedResponse(IReadOnlyList<T> items, int page, int pageSize, int totalItems)
        : this(page, pageSize, totalItems, items)
    {
    }

    /// <summary>
    /// Fábrica estática para compatibilidade com código existente.
    /// </summary>
    public static PagedResponse<T> Create(IReadOnlyList<T> items, int totalItems, int page, int pageSize)
        => new(page, pageSize, totalItems, items);
}
