using System;
using System.ComponentModel.DataAnnotations;
using BibliotecaELM.Application.Validation;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record UsuarioRequest(
    [param: Required(ErrorMessage = "O nome do usuário é obrigatório")]
    string NomeUsuario,

    [param: Required(ErrorMessage = "A data de nascimento é obrigatória")]
    [param: DateOnlyRange("1500-01-01", "2026-04-20", ErrorMessage = "A data deve estar entre 1500 e 2026")]
    DateOnly Nascimento,

    [param: Required(ErrorMessage = "O e-mail é obrigatório")]
    [param: EmailAddress(ErrorMessage = "E-mail inválido")]
    string Email,

    [param: Required(ErrorMessage = "O CPF é obrigatório")]
    [param: StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter exatamente 11 caracteres")]
    string Cpf,

    [param: Required]
    Endereco Endereco
)
{
    public Usuario ToDomain() => new Usuario(NomeUsuario, Nascimento, Email,  Cpf, Endereco);
}
