using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaELM.Domain.Enums;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record CompraResponse(Guid Id, FormaCompraEnum FormaCompra, DateTime DataCompra, Guid UsuarioId, List<Guid> LivrosIds)
{
    public static CompraResponse FromDomain(Compra compra) => new(
        compra.Id,
        compra.FormaCompra,
        compra.DataCompra,
        compra.UsuarioId,
        (compra.Livros ?? new List<Livro>()).Select(l => l.Id).ToList());
}
