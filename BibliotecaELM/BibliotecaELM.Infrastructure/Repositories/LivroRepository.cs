using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using BibliotecaELM.Infrastructure.Persistence;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class LivroRepository(BibliotecaElmContext bibliotecaElmContext) : ILivroRepository
{
    public IReadOnlyList<LivroResponse> GetAll()
    {
        return bibliotecaElmContext.Livros
            .OrderBy(f => f.Id)
            .Select(LivroResponse.FromDomain)
            .ToList();
    }
    
    public LivroResponse? GetById(Guid id)
    {
        var livro = bibliotecaElmContext.Livros
            .FirstOrDefault(f => f.Id == id);

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

    public LivroResponse? Update(Guid id, LivroRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
            throw new InvalidOperationException("O Id do Livro é obrigatório");

        if (string.IsNullOrWhiteSpace(request.NomeLivro))
            throw new InvalidOperationException("O Nome do Livro é obrigatório");

        if (request.AutorId is null || request.AutorId == Guid.Empty)
            throw new InvalidOperationException("O AutorId do livro é obrigatório");

        var autorExiste = bibliotecaElmContext.Autores
            .FirstOrDefault(a => a.Id == request.AutorId.Value) is not null;

        if (!autorExiste)
            throw new InvalidOperationException("Autor não encontrado");

        var normalizedTitle = request.NomeLivro.Trim().ToLower();
        var nomeEmUsoPorOutroLivro = bibliotecaElmContext.Livros
            .FirstOrDefault(l => l.Id != id && l.NomeLivro.ToLower() == normalizedTitle) is not null;

        if (nomeEmUsoPorOutroLivro)
            throw new InvalidOperationException("Já existe um livro com este nome");

        var livro = bibliotecaElmContext.Livros
            .FirstOrDefault(l => l.Id == id);

        if (livro is null)
            return null;

        var entry = bibliotecaElmContext.Entry(livro);
        entry.Property(l => l.NomeLivro).CurrentValue = request.NomeLivro;
        entry.Property(l => l.Preco).CurrentValue = request.Preco;
        entry.Property(l => l.DataLancamento).CurrentValue = request.DataLancamento;
        entry.Property(l => l.AutorId).CurrentValue = request.AutorId.Value;

        bibliotecaElmContext.SaveChanges();

        return LivroResponse.FromDomain(livro);
    }
    
    public bool ExistsByNomeLivro(string nomeLivro)
    {
        if (string.IsNullOrWhiteSpace(nomeLivro))
            return false;

        var normalizedTitle = nomeLivro.Trim().ToLower();
        var livro = bibliotecaElmContext.Livros
            .FirstOrDefault(f => f.NomeLivro.ToLower() == normalizedTitle);

        return livro is not null;
    }
    
    public bool ExistsById(Guid id)
    {
        var livro = bibliotecaElmContext.Livros
            .FirstOrDefault(f => f.Id == id);

        return livro is not null;
    }
    
    public bool Delete(Guid id)
    {
        var livro = bibliotecaElmContext.Livros.FirstOrDefault(f => f.Id == id);
        if (livro is null)
            return false;

        bibliotecaElmContext.Livros.Remove(livro);
        bibliotecaElmContext.SaveChanges();

        return true;
    }
    
}