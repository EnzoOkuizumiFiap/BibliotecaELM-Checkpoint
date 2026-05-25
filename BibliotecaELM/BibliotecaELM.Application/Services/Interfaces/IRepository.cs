using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Application.Services.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    IReadOnlyList<T> GetAll();
    
    T? GetById(Guid id);
    
    void Add(T entity);
    
    void Update(T entity);
    
    void Delete(T entity);
    
    bool ExistsById(Guid id);
}
