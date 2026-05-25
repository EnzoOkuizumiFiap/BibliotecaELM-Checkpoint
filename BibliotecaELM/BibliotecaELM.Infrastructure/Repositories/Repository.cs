using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Common;
using BibliotecaELM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaELM.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly BibliotecaElmContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(BibliotecaElmContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual IReadOnlyList<T> GetAll()
    {
        return _dbSet.AsNoTracking().ToList();
    }

    public virtual T? GetById(Guid id)
    {
        return _dbSet.FirstOrDefault(e => e.Id == id);
    }

    public virtual void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    public virtual bool ExistsById(Guid id)
    {
        return _dbSet.Any(e => e.Id == id);
    }
}
