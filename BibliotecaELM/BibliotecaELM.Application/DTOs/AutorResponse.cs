using System;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record AutorResponse(Guid Id, string NomeAutor, DateOnly Nascimento)
{
    public static AutorResponse FromDomain(Autor autor) => new(autor.Id, autor.NomeAutor, autor.Nascimento);
}
