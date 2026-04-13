using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record AutorRequest(
    [property: Required(ErrorMessage = "O nome do autor é obrigatório")]
    [property: StringLength(100, MinimumLength = 2, ErrorMessage = "O Nome do Autor deve ter entre 2 e 100 caracteres")]
    string NomeAutor,

    [property: Required(ErrorMessage = "A data de nascimento é obrigatória")]
    DateOnly Nascimento
)
{
    public Autor ToDomain() => new Autor(NomeAutor, Nascimento, new List<Livro>());
}
