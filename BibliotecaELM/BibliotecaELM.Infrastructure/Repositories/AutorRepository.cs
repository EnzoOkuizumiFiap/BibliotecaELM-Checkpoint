using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Infrastructure.Persistence;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class AutorRepository : Repository<Autor>, IAutorRepository
{
    public AutorRepository(BibliotecaElmContext context) : base(context)
    {
    }

    public bool ExistsByNomeAutor(string nomeAutor)
    {
        if (string.IsNullOrWhiteSpace(nomeAutor))
            return false;

        var normalizedName = nomeAutor.Trim().ToLower();
        return _context.Autores.Any(a => a.NomeAutor.ToLower() == normalizedName);
    }
}
