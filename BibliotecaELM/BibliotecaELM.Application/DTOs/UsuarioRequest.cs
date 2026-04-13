using System;
using System.ComponentModel.DataAnnotations;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record UsuarioRequest(
    [property: Required(ErrorMessage = "O nome do usuário é obrigatório")]
    string NomeUsuario,

    [property: Required(ErrorMessage = "A data de nascimento é obrigatória")]
    [property: Range(typeof(DateTime), "1500-01-01", "2026-04-20", ErrorMessage = "A data deve estar entre 1500 e 2026")]
    DateOnly Nascimento,

    [property: Required(ErrorMessage = "O e-mail é obrigatório")]
    [property: EmailAddress(ErrorMessage = "E-mail inválido")]
    string Email,

    [property: Required(ErrorMessage = "O CPF é obrigatório")]
    [property: StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter exatamente 11 caracteres")]
    string Cpf,

    [property: Required]
    EnderecoRequest Endereco
)
{
    // A conversão para domínio fica na camada de aplicação/repositório,
    // porque o Endereco depende do UsuarioId gerado no fluxo de persistência.
}
