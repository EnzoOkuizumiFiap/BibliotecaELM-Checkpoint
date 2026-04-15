using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record EmprestimoResponse(Guid Id, DateTime DataEmprestimo, DateTime DataDevolucao, Guid UsuarioId, List<Guid> LivrosIds)
{
    public static EmprestimoResponse FromDomain(Emprestimo emprestimo) => new(
        emprestimo.Id,
        emprestimo.DataEmprestimo,
        emprestimo.DataDevolucao,
        emprestimo.UsuarioId,
        (emprestimo.Livros ?? new List<Livro>()).Select(l => l.Id).ToList());
}
