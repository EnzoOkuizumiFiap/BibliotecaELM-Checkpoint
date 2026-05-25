using BibliotecaELM.Application.DTOs;

namespace BibliotecaELM.Application.Services.Interfaces;

public interface IEmprestimoService
{
    IReadOnlyList<EmprestimoResponse> GetAll();
    EmprestimoResponse? GetById(Guid id);
    EmprestimoResponse Create(EmprestimoRequest request);
    EmprestimoResponse? Update(Guid id, EmprestimoRequest request);
    bool Delete(Guid id);
}
