using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;

using BibliotecaELM.Application.Services.Interfaces;

namespace BibliotecaELM.Application.Services.Implementations;

public sealed class AutorService(IAutorRepository autorRepository) : IAutorService
{
    public IReadOnlyList<AutorResponse> GetAll()
    {
        return autorRepository.GetAll()
            .OrderBy(a => a.Id)
            .Select(AutorResponse.FromDomain)
            .ToList();
    }

    public AutorResponse? GetById(Guid id)
    {
        var autor = autorRepository.GetById(id);
        return autor is null ? null : AutorResponse.FromDomain(autor);
    }

    public AutorResponse Create(AutorRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.NomeAutor))
            throw new BusinessRuleValidationException("O Nome do Autor é obrigatório.");

        if (autorRepository.ExistsByNomeAutor(request.NomeAutor))
            throw new BusinessRuleValidationException("Já existe um autor cadastrado com este nome.");

        var autor = request.ToDomain();
        autorRepository.Add(autor);

        return AutorResponse.FromDomain(autor);
    }

    public AutorResponse? Update(Guid id, AutorRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
            throw new BusinessRuleValidationException("O Id do Autor é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.NomeAutor))
            throw new BusinessRuleValidationException("O Nome do Autor é obrigatório.");

        var autor = autorRepository.GetById(id);
        if (autor is null)
            return null;

        var normalizedName = request.NomeAutor.Trim().ToLower();
        var autorComNome = autorRepository.GetAll().FirstOrDefault(a => a.NomeAutor.Trim().ToLower() == normalizedName);
        if (autorComNome != null && autorComNome.Id != id)
            throw new BusinessRuleValidationException("Já existe um autor cadastrado com este nome.");

        autor.Update(request.NomeAutor, request.Nascimento);
        autorRepository.Update(autor);

        return AutorResponse.FromDomain(autor);
    }

    public bool ExistsByNomeAutor(string nomeAutor)
    {
        return autorRepository.ExistsByNomeAutor(nomeAutor);
    }

    public bool Delete(Guid id)
    {
        var autor = autorRepository.GetById(id);
        if (autor is null)
            return false;

        autorRepository.Delete(autor);
        return true;
    }
}
