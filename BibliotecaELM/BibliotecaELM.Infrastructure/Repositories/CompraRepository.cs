using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using BibliotecaELM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class CompraRepository(BibliotecaElmContext bibliotecaElmContext) : ICompraRepository
{
    public IReadOnlyList<CompraResponse> GetAll()
    {
        return bibliotecaElmContext.Compras
            .Include(c => c.Livros)
            .OrderBy(c => c.Id)
            .Select(CompraResponse.FromDomain)
            .ToList();
    }

    public CompraResponse? GetById(Guid id)
    {
        var compra = bibliotecaElmContext.Compras
            .Include(c => c.Livros)
            .FirstOrDefault(c => c.Id == id);

        return compra is null ? null : CompraResponse.FromDomain(compra);
    }

    public CompraResponse Create(CompraRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (!Enum.IsDefined(request.FormaCompra))
            throw new InvalidOperationException("O formato do pagamento da compra é obrigatório");

        if (request.DataCompra == default)
            throw new InvalidOperationException("A data da compra é obrigatória");

        if (request.DataCompra > DateTime.Now)
            throw new InvalidOperationException("A data da compra não pode ser futura");

        if (request.UsuarioId == Guid.Empty)
            throw new InvalidOperationException("O UsuarioId da compra é obrigatório");

        if (request.LivrosIds is null || request.LivrosIds.Count == 0)
            throw new InvalidOperationException("Ao menos um livro é obrigatório na compra");

        var usuarioExiste = bibliotecaElmContext.Usuarios
            .FirstOrDefault(u => u.Id == request.UsuarioId) is not null;

        if (!usuarioExiste)
            throw new InvalidOperationException("Usuário não encontrado");

        var usuarioTemEndereco = bibliotecaElmContext.Enderecos
            .FirstOrDefault(e => e.UsuarioId == request.UsuarioId) is not null;

        if (!usuarioTemEndereco)
            throw new InvalidOperationException("Usuário sem endereço não pode realizar compra");

        var livroIds = request.LivrosIds
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

        var compraDomain = request.ToDomain(livros);

        bibliotecaElmContext.Compras.Add(compraDomain);
        bibliotecaElmContext.SaveChanges();

        return CompraResponse.FromDomain(compraDomain);
    }

    public CompraResponse? Update(Guid id, CompraRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
            throw new InvalidOperationException("O Id da compra é obrigatório");

        if (!Enum.IsDefined(request.FormaCompra))
            throw new InvalidOperationException("O formato do pagamento da compra é obrigatório");

        if (request.DataCompra == default)
            throw new InvalidOperationException("A data da compra é obrigatória");

        if (request.DataCompra > DateTime.Now)
            throw new InvalidOperationException("A data da compra não pode ser futura");

        if (request.UsuarioId == Guid.Empty)
            throw new InvalidOperationException("O UsuarioId da compra é obrigatório");

        if (request.LivrosIds is null || request.LivrosIds.Count == 0)
            throw new InvalidOperationException("Ao menos um livro é obrigatório na compra");

        var compra = bibliotecaElmContext.Compras
            .Include(c => c.Livros)
            .FirstOrDefault(c => c.Id == id);

        if (compra is null)
            return null;

        var usuarioExiste = bibliotecaElmContext.Usuarios
            .FirstOrDefault(u => u.Id == request.UsuarioId) is not null;

        if (!usuarioExiste)
            throw new InvalidOperationException("Usuário não encontrado");

        var usuarioTemEndereco = bibliotecaElmContext.Enderecos
            .FirstOrDefault(e => e.UsuarioId == request.UsuarioId) is not null;

        if (!usuarioTemEndereco)
            throw new InvalidOperationException("Usuário sem endereço não pode realizar compra");

        var livroIds = request.LivrosIds
            .Where(livroId => livroId != Guid.Empty)
            .Distinct()
            .ToList();

        if (livroIds.Count == 0)
            throw new InvalidOperationException("Os livros da compra são inválidos");

        var livros = bibliotecaElmContext.Livros
            .Where(l => livroIds.Contains(l.Id))
            .ToList();

        if (livros.Count != livroIds.Count)
            throw new InvalidOperationException("Um ou mais livros não foram encontrados");

        var compraEntry = bibliotecaElmContext.Entry(compra);
        compraEntry.Property(c => c.FormaCompra).CurrentValue = request.FormaCompra;
        compraEntry.Property(c => c.DataCompra).CurrentValue = request.DataCompra;
        compraEntry.Property(c => c.UsuarioId).CurrentValue = request.UsuarioId;

        if (compra.Livros is null)
        {
            compraEntry.Collection(c => c.Livros).CurrentValue = new List<Domain.Entities.Livro>();
        }

        var livrosDaCompra = compra.Livros ?? new List<Domain.Entities.Livro>();
        livrosDaCompra.Clear();
        foreach (var livro in livros)
        {
            livrosDaCompra.Add(livro);
        }

        compraEntry.Collection(c => c.Livros).CurrentValue = livrosDaCompra;

        bibliotecaElmContext.SaveChanges();

        return CompraResponse.FromDomain(compra);
    }

    public bool ExistsById(Guid id)
    {
        return bibliotecaElmContext.Compras.FirstOrDefault(c => c.Id == id) is not null;
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