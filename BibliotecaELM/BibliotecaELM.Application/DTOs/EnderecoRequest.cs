using System;
using System.ComponentModel.DataAnnotations;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record EnderecoRequest(
    [param: Required(ErrorMessage = "O CEP é obrigatório")]
    string Cep,

    [param: Required(ErrorMessage = "O estado é obrigatório")]
    string Estado,

    [param: Required(ErrorMessage = "A cidade é obrigatória")]
    string Cidade,

    [param: Required(ErrorMessage = "O bairro é obrigatório")]
    string Bairro,

    [param: Required(ErrorMessage = "A rua é obrigatória")]
    string Rua,
    
    [param: Required(ErrorMessage = "O AutorId é obrigatório")]
    Guid UsuarioId
)
{
    public Endereco ToDomain() => new Endereco(Cep, Estado, Cidade, Bairro, Rua, UsuarioId);
}
