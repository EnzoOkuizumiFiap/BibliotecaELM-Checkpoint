using BibliotecaELM.Application.DTOs;

namespace BibliotecaELM.Application.Services;

public interface ILivroRepository
{
    IReadOnlyList<LivroResponse> GetAll();
    
    LivroResponse? GetById(Guid id);
    
    LivroResponse Create(LivroRequest request);
    
    bool ExistsByNomeLivro(string nomeLivro);
    
    bool ExistsById(Guid id);
    
    bool Delete(Guid id);
}