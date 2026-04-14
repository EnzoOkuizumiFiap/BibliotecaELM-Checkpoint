using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using BibliotecaELM.Infrastructure.Persistence;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class CompraRepository(BibliotecaElmContext bibliotecaElmContext) : ICompraRepository
{
    public IReadOnlyList<CompraResponse> GetAll()
    {
        return bibliotecaElmContext.Compras
            .OrderBy(c => c.Id)
            .Select(CompraResponse.FromDomain)
            .ToList();
    }

    public CompraResponse? GetById(Guid id)
    {
        var compra = bibliotecaElmContext.Compras
            .FirstOrDefault(c => c.Id == id);

        return compra is null ? null : CompraResponse.FromDomain(compra);
    }

    public CompraResponse Create(CompraRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (!Enum.IsDefined(request.FormaCompra))
            throw new InvalidOperationException("O formato do pagamento da compra é obrigatório");

        if (request.UsuarioId == Guid.Empty)
            throw new InvalidOperationException("O UsuarioId da compra é obrigatório");

        if (request.Livros is null || request.Livros.Count == 0)
            throw new InvalidOperationException("Ao menos um livro é obrigatório na compra");

        var usuarioExiste = bibliotecaElmContext.Usuarios
            .Any(u => u.Id == request.UsuarioId);

        if (!usuarioExiste)
            throw new InvalidOperationException("Usuário não encontrado");

        var livroIds = request.Livros
            .Select(l => l.Id)
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (livroIds.Count == 0)
            throw new InvalidOperationException("Os livros da compra são inválidos");

        var livros = bibliotecaElmContext.Livros
            .Where(l => livroIds.Contains(l.Id))
            .ToList();

        if (livros.Count != livroIds.Count)
            throw new InvalidOperationException("Um ou mais livros não foram encontrados");

        var compra = request with { Livros = livros };
        var compraDomain = compra.ToDomain();

        bibliotecaElmContext.Compras.Add(compraDomain);
        bibliotecaElmContext.SaveChanges();

        return CompraResponse.FromDomain(compraDomain);
    }

    public bool ExistsById(Guid id)
    {
        return bibliotecaElmContext.Compras.Any(c => c.Id == id);
    }

    public bool Delete(Guid id)
    {
        var compra = bibliotecaElmContext.Compras.FirstOrDefault(c => c.Id == id);
        if (compra is null)
            return false;

        bibliotecaElmContext.Compras.Remove(compra);
        bibliotecaElmContext.SaveChanges();

        return true;
    }
}