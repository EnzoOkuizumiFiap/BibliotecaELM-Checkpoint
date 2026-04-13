using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record LivroResponse (Guid Id, string NomeLivro)
{
    public static LivroResponse FromDomain(Livro livro) => new(livro.Id, livro.NomeLivro);
}