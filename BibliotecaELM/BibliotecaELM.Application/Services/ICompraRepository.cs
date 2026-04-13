using BibliotecaELM.Application.DTOs;

namespace BibliotecaELM.Application.Services;

public interface ICompraRepository
{
    IReadOnlyList<CompraResponse> GetAll();

    CompraResponse? GetById(Guid id);

    CompraResponse Create(CompraRequest request);

    bool ExistsById(Guid id);

    bool Delete(Guid id);
}
