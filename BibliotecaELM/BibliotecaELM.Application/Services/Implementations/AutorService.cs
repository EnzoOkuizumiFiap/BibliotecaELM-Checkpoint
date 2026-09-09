using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BibliotecaELM.Application.Services.Implementations;

public sealed class AutorService(
    IAutorRepository autorRepository,
    ILogger<AutorService>? logger = null) : IAutorService
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
        {
            logger?.LogWarning("Tentativa de criação de autor falhou: Nome do autor é obrigatório.");
            throw new BusinessRuleValidationException("O Nome do Autor é obrigatório.");
        }

        if (autorRepository.ExistsByNomeAutor(request.NomeAutor))
        {
            logger?.LogWarning("Tentativa de criação de autor falhou: Nome {NomeAutor} já cadastrado.", request.NomeAutor);
            throw new BusinessRuleValidationException("Já existe um autor cadastrado com este nome.");
        }

        var autor = request.ToDomain();
        autorRepository.Add(autor);

        logger?.LogInformation("Autor {AutorId} ({NomeAutor}) criado com sucesso no repositório.", autor.Id, autor.NomeAutor);

        return AutorResponse.FromDomain(autor);
    }

    public AutorResponse? Update(Guid id, AutorRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
        {
            logger?.LogWarning("Tentativa de atualização de autor falhou: Id vazio.");
            throw new BusinessRuleValidationException("O Id do Autor é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.NomeAutor))
        {
            logger?.LogWarning("Tentativa de atualização de autor {AutorId} falhou: Nome vazio.", id);
            throw new BusinessRuleValidationException("O Nome do Autor é obrigatório.");
        }

        var autor = autorRepository.GetById(id);
        if (autor is null)
        {
            logger?.LogWarning("Tentativa de atualização falhou: Autor {AutorId} não encontrado.", id);
            return null;
        }

        var normalizedName = request.NomeAutor.Trim().ToLower();
        var autorComNome = autorRepository.GetAll().FirstOrDefault(a => a.NomeAutor.Trim().ToLower() == normalizedName);
        if (autorComNome != null && autorComNome.Id != id)
        {
            logger?.LogWarning("Tentativa de atualização do autor {AutorId} falhou: Nome {NomeAutor} já utilizado.", id, request.NomeAutor);
            throw new BusinessRuleValidationException("Já existe um autor cadastrado com este nome.");
        }

        autor.Update(request.NomeAutor, request.Nascimento);
        autorRepository.Update(autor);

        logger?.LogInformation("Autor {AutorId} ({NomeAutor}) atualizado com sucesso no repositório.", autor.Id, autor.NomeAutor);

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
        {
            logger?.LogWarning("Tentativa de exclusão falhou: Autor {AutorId} não encontrado.", id);
            return false;
        }

        autorRepository.Delete(autor);
        logger?.LogInformation("Autor {AutorId} removido com sucesso do repositório.", id);
        return true;
    }
}
