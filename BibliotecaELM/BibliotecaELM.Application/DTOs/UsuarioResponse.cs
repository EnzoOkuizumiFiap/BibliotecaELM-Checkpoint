using System;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record UsuarioResponse(Guid Id, string NomeUsuario, DateOnly Nascimento, string Email, string Cpf, EnderecoResponse Endereco)
{
    public static UsuarioResponse FromDomain(Usuario usuario) => new(usuario.Id, usuario.NomeUsuario, usuario.Nascimento, usuario.Email, usuario.Cpf, EnderecoResponse.FromDomain(usuario.Endereco));
}
