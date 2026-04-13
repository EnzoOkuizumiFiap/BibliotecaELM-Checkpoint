using System;
using System.ComponentModel.DataAnnotations;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record EnderecoRequest(
    [property: Required(ErrorMessage = "O CEP é obrigatório")]
    string Cep,

    [property: Required(ErrorMessage = "O estado é obrigatório")]
    string Estado,

    [property: Required(ErrorMessage = "A cidade é obrigatória")]
    string Cidade,

    [property: Required(ErrorMessage = "O bairro é obrigatório")]
    string Bairro,

    [property: Required(ErrorMessage = "A rua é obrigatória")]
    string Rua
)
{
    public Endereco ToDomain(Guid usuarioId) => new Endereco(Cep, Estado, Cidade, Bairro, Rua, usuarioId);
}
