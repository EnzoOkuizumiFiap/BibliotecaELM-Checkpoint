using BibliotecaELM.Application.DTOs;

namespace BibliotecaELM.Application.Services;

public interface IAutorRepository
{
    IReadOnlyList<AutorResponse> GetAll();

    AutorResponse? GetById(Guid id);

    AutorResponse Create(AutorRequest request);

    AutorResponse? Update(Guid id, AutorRequest request);

    bool ExistsByNomeAutor(string nomeAutor);

    bool ExistsById(Guid id);

    bool Delete(Guid id);
}
