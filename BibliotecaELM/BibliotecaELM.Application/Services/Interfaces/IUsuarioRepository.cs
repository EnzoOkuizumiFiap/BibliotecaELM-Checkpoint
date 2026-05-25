using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.Services.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    bool ExistsByEmail(string email);
}
