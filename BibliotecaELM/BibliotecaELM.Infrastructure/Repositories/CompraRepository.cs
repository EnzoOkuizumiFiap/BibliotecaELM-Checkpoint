using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class CompraRepository : Repository<Compra>, ICompraRepository
{
    public CompraRepository(BibliotecaElmContext context) : base(context)
    {
    }

    public override IReadOnlyList<Compra> GetAll()
    {
        return _dbSet.AsNoTracking()
            .Include(c => c.Livros)
            .OrderBy(c => c.Id)
            .ToList();
    }

    public override Compra? GetById(Guid id)
    {
        return _dbSet
            .Include(c => c.Livros)
            .FirstOrDefault(c => c.Id == id);
    }
}
