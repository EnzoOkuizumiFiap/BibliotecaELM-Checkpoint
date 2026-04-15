using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record EmprestimoRequest(
    [param: Required(ErrorMessage = "A data de empréstimo é obrigatória")]
    DateTime DataEmprestimo,

    [param: Required(ErrorMessage = "A data de devolução é obrigatória")]
    DateTime DataDevolucao,

    [param: Required(ErrorMessage = "O UsuarioId é obrigatório")]
    Guid UsuarioId,

    [param: Required(ErrorMessage = "Os IDs dos livros são obrigatórios")]
    [param: MinLength(1, ErrorMessage = "Ao menos um livro é obrigatório no empréstimo")]
    List<Guid> LivrosIds
)
{
    public Emprestimo ToDomain(List<Livro> livros) => new Emprestimo(DataEmprestimo, DataDevolucao, UsuarioId, livros);
}
