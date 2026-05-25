using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.Services.Interfaces;

public interface IAutorRepository : IRepository<Autor>
{
    bool ExistsByNomeAutor(string nomeAutor);
}
