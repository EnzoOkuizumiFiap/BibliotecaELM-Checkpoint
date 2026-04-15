using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record LivroResponse (Guid Id, string NomeLivro, decimal Preco, DateOnly DataLancamento, Guid AutorId)
{
    public static LivroResponse FromDomain(Livro livro) => new(livro.Id, livro.NomeLivro, livro.Preco, livro.DataLancamento, livro.AutorId);
}