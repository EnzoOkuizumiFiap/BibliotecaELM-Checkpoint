using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BibliotecaELM.Application.Validation;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record AutorRequest(
    [param: Required(ErrorMessage = "O nome do autor é obrigatório")]
    [param: StringLength(100, MinimumLength = 2, ErrorMessage = "O Nome do Autor deve ter entre 2 e 100 caracteres")]
    string NomeAutor,

    [param: Required(ErrorMessage = "A data de nascimento é obrigatória")]
    [param: DateOnlyRange("1500-01-01", "2026-04-20", ErrorMessage = "A data deve estar entre 1500 e 2026")]
    DateOnly Nascimento
)
{
    public Autor ToDomain() => new Autor(NomeAutor, Nascimento, new List<Livro>());
}
