using BibliotecaELM.Application.DTOs;

namespace BibliotecaELM.Application.Services.Interfaces;

public interface IAutorService
{
    IReadOnlyList<AutorResponse> GetAll();
    AutorResponse? GetById(Guid id);
    AutorResponse Create(AutorRequest request);
    AutorResponse? Update(Guid id, AutorRequest request);
    bool ExistsByNomeAutor(string nomeAutor);
    bool Delete(Guid id);
}
