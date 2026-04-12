using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using BibliotecaELM.Infrastructure.Persistence;

namespace BibliotecaELM.Infrastructure;

public sealed class LivroRepository(BibliotecaElmContext bibliotecaElmContext) : ILivroRepository
{
    public IReadOnlyList<LivroResponse> GetAll()
    {
        return bibliotecaElmContext.Livros
            .OrderBy(m => m.NomeLivro)
            .Select(LivroResponse.FromDomain)
            .ToList();
    }
    
    public LivroResponse? GetById(Guid id)
    {
        var livro = bibliotecaElmContext.Livros
            .FirstOrDefault(m => m.Id == id);

        return livro is null ? null : LivroResponse.FromDomain(livro);
    }
    
    public LivroResponse Create(LivroRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.NomeLivro))
            throw new InvalidOperationException("O Nome do Livro é obrigatório");

        if (ExistsByNomeLivro(request.NomeLivro))
            throw new InvalidOperationException("Já existe um livro com este nome");

        var livro = request.ToDomain();

        bibliotecaElmContext.Livros.Add(livro);
        bibliotecaElmContext.SaveChanges();

        return LivroResponse.FromDomain(livro);
    }
    
    public bool ExistsByNomeLivro(string nomeLivro)
    {
        if (string.IsNullOrWhiteSpace(nomeLivro))
            return false;

        var normalizedTitle = nomeLivro.Trim().ToLower();

        return bibliotecaElmContext.Livros
            .Any(m => m.NomeLivro.ToLower() == normalizedTitle);
    }
    
    public bool ExistsById(Guid id)
    {
        return bibliotecaElmContext.Livros
            .Any(m => m.Id == id);
    }
    
    public bool Delete(Guid id)
    {
        var movie = bibliotecaElmContext.Livros.FirstOrDefault(m => m.Id == id);
        if (movie is null)
            return false;

        bibliotecaElmContext.Livros.Remove(movie);
        bibliotecaElmContext.SaveChanges();

        return true;
    }
    
}