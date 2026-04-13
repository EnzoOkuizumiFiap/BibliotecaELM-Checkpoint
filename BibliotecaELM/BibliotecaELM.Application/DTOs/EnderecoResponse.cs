using System;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record EnderecoResponse(Guid Id, string Cep, string Estado, string Cidade, string Bairro, string Rua, Guid UsuarioId)
{
    public static EnderecoResponse FromDomain(Endereco endereco) => new(endereco.Id, endereco.Cep, endereco.Estado, endereco.Cidade, endereco.Bairro, endereco.Rua, endereco.UsuarioId);
}
