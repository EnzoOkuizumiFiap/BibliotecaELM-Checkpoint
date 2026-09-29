using BibliotecaELM.Application.DTOs;

namespace BibliotecaELM.Application.Services.Interfaces;

public interface ILivroService
{
    IReadOnlyList<LivroResponse> GetAll();
    PagedResponse<LivroResponse> GetPaged(int page, int pageSize);
    LivroResponse? GetById(Guid id);
    LivroResponse Create(LivroRequest request);
    LivroResponse? Update(Guid id, LivroRequest request);
    bool ExistsByNomeLivro(string nomeLivro);
    bool Delete(Guid id);
}
