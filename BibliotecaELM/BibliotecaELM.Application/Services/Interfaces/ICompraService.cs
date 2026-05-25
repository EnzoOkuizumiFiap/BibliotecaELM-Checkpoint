using BibliotecaELM.Application.DTOs;

namespace BibliotecaELM.Application.Services.Interfaces;

public interface ICompraService
{
    IReadOnlyList<CompraResponse> GetAll();
    CompraResponse? GetById(Guid id);
    CompraResponse Create(CompraRequest request);
    CompraResponse? Update(Guid id, CompraRequest request);
    bool Delete(Guid id);
}
