using BibliotecaELM.Application.DTOs;

namespace BibliotecaELM.Application.Services;

public interface IEnderecoRepository
{
    IReadOnlyList<EnderecoResponse> GetAll();

    EnderecoResponse? GetById(Guid id);

    EnderecoResponse Create(EnderecoRequest request, Guid usuarioId);

    bool ExistsById(Guid id);
    
    bool ExistsByIdUsuario(Guid usuarioId);

    bool Delete(Guid id);
}
