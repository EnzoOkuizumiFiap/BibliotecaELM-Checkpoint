using BibliotecaELM.Application.DTOs;

namespace BibliotecaELM.Application.Services.Interfaces;

public interface IEnderecoService
{
    IReadOnlyList<EnderecoResponse> GetAll();
    EnderecoResponse? GetById(Guid id);
    EnderecoResponse Create(EnderecoRequest request, Guid usuarioId);
    EnderecoResponse? Update(Guid id, EnderecoRequest request);
    bool Delete(Guid id);
}
