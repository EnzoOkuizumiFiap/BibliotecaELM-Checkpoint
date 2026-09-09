using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BibliotecaELM.Application.Services.Implementations;

public sealed class LivroService(
    ILivroRepository livroRepository,
    IAutorRepository autorRepository,
    ILogger<LivroService>? logger = null) : ILivroService
{
    public IReadOnlyList<LivroResponse> GetAll()
    {
        return livroRepository.GetAll()
            .OrderBy(l => l.Id)
            .Select(LivroResponse.FromDomain)
            .ToList();
    }

    public LivroResponse? GetById(Guid id)
    {
        var livro = livroRepository.GetById(id);
        return livro is null ? null : LivroResponse.FromDomain(livro);
    }

    public LivroResponse Create(LivroRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.NomeLivro))
        {
            logger?.LogWarning("Tentativa de criação de livro falhou: Nome do livro é obrigatório.");
            throw new BusinessRuleValidationException("O Nome do Livro é obrigatório.");
        }

        if (request.AutorId == null || request.AutorId == Guid.Empty)
        {
            logger?.LogWarning("Tentativa de criação de livro falhou: AutorId é obrigatório.");
            throw new BusinessRuleValidationException("O AutorId do livro é obrigatório.");
        }

        if (livroRepository.ExistsByNomeLivro(request.NomeLivro))
        {
            logger?.LogWarning("Tentativa de criação de livro falhou: Livro {NomeLivro} já cadastrado.", request.NomeLivro);
            throw new BusinessRuleValidationException("Já existe um livro cadastrado com este nome.");
        }

        var autorExiste = autorRepository.ExistsById(request.AutorId.Value);
        if (!autorExiste)
        {
            logger?.LogWarning("Tentativa de criação de livro falhou: Autor {AutorId} não encontrado.", request.AutorId.Value);
            throw new ResourceNotFoundException("Autor não encontrado.");
        }

        var livro = request.ToDomain();
        livroRepository.Add(livro);

        logger?.LogInformation("Livro {LivroId} ({NomeLivro}) cadastrado com sucesso no repositório.", livro.Id, livro.NomeLivro);

        return LivroResponse.FromDomain(livro);
    }

    public LivroResponse? Update(Guid id, LivroRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
        {
            logger?.LogWarning("Tentativa de atualização de livro falhou: Id vazio.");
            throw new BusinessRuleValidationException("O Id do livro é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.NomeLivro))
        {
            logger?.LogWarning("Tentativa de atualização de livro {LivroId} falhou: Nome vazio.", id);
            throw new BusinessRuleValidationException("O Nome do Livro é obrigatório.");
        }

        if (request.AutorId == null || request.AutorId == Guid.Empty)
        {
            logger?.LogWarning("Tentativa de atualização de livro {LivroId} falhou: AutorId vazio.", id);
            throw new BusinessRuleValidationException("O AutorId do livro é obrigatório.");
        }

        var livro = livroRepository.GetById(id);
        if (livro is null)
        {
            logger?.LogWarning("Tentativa de atualização falhou: Livro {LivroId} não encontrado.", id);
            return null;
        }

        var normalizedName = request.NomeLivro.Trim().ToLower();
        var livroComNome = livroRepository.GetAll().FirstOrDefault(l => l.NomeLivro.Trim().ToLower() == normalizedName);
        if (livroComNome != null && livroComNome.Id != id)
        {
            logger?.LogWarning("Tentativa de atualização do livro {LivroId} falhou: Nome {NomeLivro} já utilizado.", id, request.NomeLivro);
            throw new BusinessRuleValidationException("Já existe um livro cadastrado com este nome.");
        }

        var autorExiste = autorRepository.ExistsById(request.AutorId.Value);
        if (!autorExiste)
        {
            logger?.LogWarning("Tentativa de atualização do livro {LivroId} falhou: Autor {AutorId} não encontrado.", id, request.AutorId.Value);
            throw new ResourceNotFoundException("Autor não encontrado.");
        }

        livro.Update(request.NomeLivro, request.Preco, request.DataLancamento, request.AutorId.Value);
        livroRepository.Update(livro);

        logger?.LogInformation("Livro {LivroId} ({NomeLivro}) atualizado com sucesso no repositório.", livro.Id, livro.NomeLivro);

        return LivroResponse.FromDomain(livro);
    }

    public bool ExistsByNomeLivro(string nomeLivro)
    {
        return livroRepository.ExistsByNomeLivro(nomeLivro);
    }

    public bool Delete(Guid id)
    {
        var livro = livroRepository.GetById(id);
        if (livro is null)
        {
            logger?.LogWarning("Tentativa de exclusão falhou: Livro {LivroId} não encontrado.", id);
            return false;
        }

        livroRepository.Delete(livro);
        logger?.LogInformation("Livro {LivroId} removido com sucesso do repositório.", id);
        return true;
    }
}
