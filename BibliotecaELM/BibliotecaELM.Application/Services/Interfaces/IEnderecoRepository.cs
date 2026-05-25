using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.Services.Interfaces;

public interface IEnderecoRepository : IRepository<Endereco>
{
    bool ExistsByIdUsuario(Guid usuarioId);
}
