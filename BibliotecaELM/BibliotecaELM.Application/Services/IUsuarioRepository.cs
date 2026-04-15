using BibliotecaELM.Application.DTOs;

namespace BibliotecaELM.Application.Services;

public interface IUsuarioRepository
{
    IReadOnlyList<UsuarioResponse> GetAll();

    UsuarioResponse? GetById(Guid id);

    UsuarioResponse Create(UsuarioRequest request);

    UsuarioResponse? Update(Guid id, UsuarioRequest request);

    bool ExistsByEmail(string email);

    bool ExistsById(Guid id);

    bool Delete(Guid id);
}
