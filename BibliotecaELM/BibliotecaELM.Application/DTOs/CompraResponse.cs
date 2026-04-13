using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Enums;

namespace BibliotecaELM.Application.DTOs;

public record CompraResponse(Guid Id, FormaCompraEnum FormaCompra, DateTime DataCompra, Guid UsuarioId, List<Guid> LivroIds)
{
    public static CompraResponse FromDomain(Compra compra) => new(compra.Id, compra.FormaCompra, compra.DataCompra, compra.UsuarioId, compra.Livros.Select(l => l.Id).ToList());
}
