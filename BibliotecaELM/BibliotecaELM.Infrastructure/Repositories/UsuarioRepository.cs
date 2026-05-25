using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(BibliotecaElmContext context) : base(context)
    {
    }

    public override IReadOnlyList<Usuario> GetAll()
    {
        return _dbSet.AsNoTracking()
            .Include(u => u.Endereco)
            .OrderBy(u => u.Id)
            .ToList();
    }

    public override Usuario? GetById(Guid id)
    {
        return _dbSet
            .Include(u => u.Endereco)
            .FirstOrDefault(u => u.Id == id);
    }

    public bool ExistsByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var normalizedEmail = email.Trim().ToLower();
        return _context.Usuarios.Any(u => u.Email.ToLower() == normalizedEmail);
    }
}
