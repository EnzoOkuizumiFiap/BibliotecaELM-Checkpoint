using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class EmprestimoRepository : Repository<Emprestimo>, IEmprestimoRepository
{
    public EmprestimoRepository(BibliotecaElmContext context) : base(context)
    {
    }

    public override IReadOnlyList<Emprestimo> GetAll()
    {
        return _dbSet.AsNoTracking()
            .Include(e => e.Livros)
            .OrderBy(e => e.Id)
            .ToList();
    }

    public override Emprestimo? GetById(Guid id)
    {
        return _dbSet
            .Include(e => e.Livros)
            .FirstOrDefault(e => e.Id == id);
    }
}
