using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record EmprestimoRequest(
    [param: Required(ErrorMessage = "A data de empréstimo é obrigatória")]
    DateTime DataEmprestimo,

    [param: Required(ErrorMessage = "A data de devolução é obrigatória")]
    DateTime DataDevolucao,

    [param: Required(ErrorMessage = "O UsuarioId é obrigatório")]
    Guid UsuarioId,

    [param: Required(ErrorMessage = "Os livros são obrigatórios")]
    [param: MinLength(1, ErrorMessage = "Ao menos um livro é obrigatório no empréstimo")]
    List<Livro> Livros
)
{
    public Emprestimo ToDomain() => new Emprestimo(DataEmprestimo, DataDevolucao, UsuarioId, Livros);
}
