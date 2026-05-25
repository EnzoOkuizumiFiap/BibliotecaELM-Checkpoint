using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.Services.Interfaces;

public interface ILivroRepository : IRepository<Livro>
{
    bool ExistsByNomeLivro(string nomeLivro);
}
