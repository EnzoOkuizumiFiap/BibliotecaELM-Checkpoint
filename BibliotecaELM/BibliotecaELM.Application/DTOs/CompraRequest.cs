using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Enums;

namespace BibliotecaELM.Application.DTOs;

public record CompraRequest(
    [property: Required(ErrorMessage = "A forma de compra é obrigatória")]
    FormaCompraEnum FormaCompra,

    [property: Required(ErrorMessage = "A data da compra é obrigatória")]
    DateTime DataCompra,

    [property: Required(ErrorMessage = "O UsuarioId é obrigatório")]
    Guid UsuarioId,

    [property: Required(ErrorMessage = "Os Ids dos livros são obrigatórios")]
    [property: MinLength(1, ErrorMessage = "Ao menos um livro é obrigatório na compra")]
    List<Guid> LivroIds
)
{
    public Compra ToDomain(IEnumerable<Livro> livros) => new Compra(FormaCompra, DataCompra, UsuarioId, livros.ToList());
}
