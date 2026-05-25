using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Infrastructure.Persistence;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class LivroRepository : Repository<Livro>, ILivroRepository
{
    public LivroRepository(BibliotecaElmContext context) : base(context)
    {
    }

    public bool ExistsByNomeLivro(string nomeLivro)
    {
        if (string.IsNullOrWhiteSpace(nomeLivro))
            return false;

        var normalizedName = nomeLivro.Trim().ToLower();
        return _context.Livros.Any(a => a.NomeLivro.ToLower() == normalizedName);
    }
}
