using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using BibliotecaELM.Infrastructure.Persistence;
namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class AutorRepository(BibliotecaElmContext bibliotecaElmContext) : IAutorRepository
{
    public IReadOnlyList<AutorResponse> GetAll()
    {
        return bibliotecaElmContext.Autores
            .OrderBy(a => a.Id)
            .Select(AutorResponse.FromDomain)
            .ToList();
    }

    public AutorResponse? GetById(Guid id)
    {
        var autor = bibliotecaElmContext.Autores
            .FirstOrDefault(a => a.Id == id);

        return autor is null ? null : AutorResponse.FromDomain(autor);
    }

    public AutorResponse Create(AutorRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.NomeAutor))
            throw new InvalidOperationException("O Nome do Autor é obrigatório");

        if (ExistsByNomeAutor(request.NomeAutor))
            throw new InvalidOperationException("Já existe um autor com este nome");

        var autor = request.ToDomain();

        bibliotecaElmContext.Autores.Add(autor);
        bibliotecaElmContext.SaveChanges();

        return AutorResponse.FromDomain(autor);
    }

    public AutorResponse? Update(Guid id, AutorRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
            throw new InvalidOperationException("O Id do Autor é obrigatório");

        if (string.IsNullOrWhiteSpace(request.NomeAutor))
            throw new InvalidOperationException("O Nome do Autor é obrigatório");

        var normalizedName = request.NomeAutor.Trim().ToLower();
        var nomeEmUsoPorOutroAutor = bibliotecaElmContext.Autores
            .FirstOrDefault(a => a.Id != id && a.NomeAutor.ToLower() == normalizedName) is not null;

        if (nomeEmUsoPorOutroAutor)
            throw new InvalidOperationException("Já existe um autor com este nome");

        var autor = bibliotecaElmContext.Autores
            .FirstOrDefault(a => a.Id == id);

        if (autor is null)
            return null;

        var entry = bibliotecaElmContext.Entry(autor);
        entry.Property(a => a.NomeAutor).CurrentValue = request.NomeAutor;
        entry.Property(a => a.Nascimento).CurrentValue = request.Nascimento;

        bibliotecaElmContext.SaveChanges();

        return AutorResponse.FromDomain(autor);
    }

    public bool ExistsByNomeAutor(string nomeAutor)
    {
        if (string.IsNullOrWhiteSpace(nomeAutor))
            return false;

        var normalizedName = nomeAutor.Trim().ToLower();
        var autor = bibliotecaElmContext.Autores
            .FirstOrDefault(a => a.NomeAutor.ToLower() == normalizedName);

        return autor is not null;
    }

    public bool ExistsById(Guid id)
    {
        var autor = bibliotecaElmContext.Autores
            .FirstOrDefault(a => a.Id == id);

        return autor is not null;
    }

    public bool Delete(Guid id)
    {
        var autor = bibliotecaElmContext.Autores.FirstOrDefault(a => a.Id == id);
        if (autor is null)
            return false;

        bibliotecaElmContext.Autores.Remove(autor);
        bibliotecaElmContext.SaveChanges();

        return true;
    }
}
