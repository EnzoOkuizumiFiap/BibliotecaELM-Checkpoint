using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Application.Services.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    IReadOnlyList<T> GetAll();

    (IReadOnlyList<T> Items, int TotalItems) GetPaged(int page, int pageSize, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

    T? GetById(Guid id);
    
    void Add(T entity);
    
    void Update(T entity);
    
    void Delete(T entity);
    
    bool ExistsById(Guid id);
}
