using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record LivroResponse (Guid id, string NomeLivro)
{
    public static LivroResponse FromDomain(Livro livro) => new(livro.Id, livro.NomeLivro);
}