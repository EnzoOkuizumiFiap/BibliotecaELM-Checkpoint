using System.Collections;
using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;

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

        if (string.IsNullOrWhiteSpace(request.FormaCompra.ToString()))
            throw new InvalidOperationException("O formato do pagamento da compra é obrigatório");

        IEnumerable<Livro> livros = bibliotecaElmContext.Livros;
        
        var compra = request.ToDomain(livros);

        bibliotecaElmContext.Compras.Add(compra);
        bibliotecaElmContext.SaveChanges();

        return CompraResponse.FromDomain(compra);
    }

    public bool ExistsById(Guid id)
    {
        var compra = bibliotecaElmContext.Compras
            .FirstOrDefault(c => c.Id == id);

        return compra is not null;
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