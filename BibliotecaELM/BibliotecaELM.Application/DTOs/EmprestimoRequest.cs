using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record EmprestimoRequest(
    [property: Required(ErrorMessage = "A data de empréstimo é obrigatória")]
    DateTime DataEmprestimo,

    [property: Required(ErrorMessage = "A data de devolução é obrigatória")]
    DateTime DataDevolucao,

    [property: Required(ErrorMessage = "O UsuarioId é obrigatório")]
    Guid UsuarioId,

    [property: Required(ErrorMessage = "Os Ids dos livros são obrigatórios")]
    [property: MinLength(1, ErrorMessage = "Ao menos um livro é obrigatório no empréstimo")]
    List<Guid> LivroIds
)
{
    public Emprestimo ToDomain(IEnumerable<Livro> livros) => new Emprestimo(DataEmprestimo, DataDevolucao, UsuarioId, livros.ToList());
}
