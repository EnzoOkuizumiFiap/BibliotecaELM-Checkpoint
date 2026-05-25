using BibliotecaELM.Application.DTOs;

namespace BibliotecaELM.Application.Services.Interfaces;

public interface IUsuarioService
{
    IReadOnlyList<UsuarioResponse> GetAll();
    UsuarioResponse? GetById(Guid id);
    UsuarioResponse Create(UsuarioRequest request);
    UsuarioResponse? Update(Guid id, UsuarioRequest request);
    bool ExistsByEmail(string email);
    bool Delete(Guid id);
}
