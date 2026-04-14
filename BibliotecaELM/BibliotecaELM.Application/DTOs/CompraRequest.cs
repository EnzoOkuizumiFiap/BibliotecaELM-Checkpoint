using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Enums;

namespace BibliotecaELM.Application.DTOs;

public record CompraRequest(
    [param: Required(ErrorMessage = "A forma de compra é obrigatória")]
    FormaCompraEnum FormaCompra,

    [param: Required(ErrorMessage = "A data da compra é obrigatória")]
    DateTime DataCompra,

    [param: Required(ErrorMessage = "O UsuarioId é obrigatório")]
    Guid UsuarioId,

    [param: Required(ErrorMessage = "Os livros são obrigatórios")]
    [param: MinLength(1, ErrorMessage = "Ao menos um livro é obrigatório na compra")]
    List<Livro> Livros
)
{
    public Compra ToDomain() => new Compra(FormaCompra, DataCompra, UsuarioId, Livros);
}
