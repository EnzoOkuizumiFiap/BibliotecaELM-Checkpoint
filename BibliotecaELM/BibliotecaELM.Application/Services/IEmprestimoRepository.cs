using BibliotecaELM.Application.DTOs;

namespace BibliotecaELM.Application.Services;

public interface IEmprestimoRepository
{
    IReadOnlyList<EmprestimoResponse> GetAll();

    EmprestimoResponse? GetById(Guid id);

    EmprestimoResponse Create(EmprestimoRequest request);

    bool ExistsById(Guid id);

    bool Delete(Guid id);
}
