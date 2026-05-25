using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;

using BibliotecaELM.Application.Services.Interfaces;

namespace BibliotecaELM.Application.Services.Implementations;

public sealed class LivroService(
    ILivroRepository livroRepository,
    IAutorRepository autorRepository) : ILivroService
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
            throw new BusinessRuleValidationException("O Nome do Livro é obrigatório.");

        if (request.AutorId == null || request.AutorId == Guid.Empty)
            throw new BusinessRuleValidationException("O AutorId do livro é obrigatório.");

        if (livroRepository.ExistsByNomeLivro(request.NomeLivro))
            throw new BusinessRuleValidationException("Já existe um livro cadastrado com este nome.");

        var autorExiste = autorRepository.ExistsById(request.AutorId.Value);
        if (!autorExiste)
            throw new ResourceNotFoundException("Autor não encontrado.");

        var livro = request.ToDomain();
        livroRepository.Add(livro);

        return LivroResponse.FromDomain(livro);
    }

    public LivroResponse? Update(Guid id, LivroRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
            throw new BusinessRuleValidationException("O Id do livro é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.NomeLivro))
            throw new BusinessRuleValidationException("O Nome do Livro é obrigatório.");

        if (request.AutorId == null || request.AutorId == Guid.Empty)
            throw new BusinessRuleValidationException("O AutorId do livro é obrigatório.");

        var livro = livroRepository.GetById(id);
        if (livro is null)
            return null;

        var normalizedName = request.NomeLivro.Trim().ToLower();
        var livroComNome = livroRepository.GetAll().FirstOrDefault(l => l.NomeLivro.Trim().ToLower() == normalizedName);
        if (livroComNome != null && livroComNome.Id != id)
            throw new BusinessRuleValidationException("Já existe um livro cadastrado com este nome.");

        var autorExiste = autorRepository.ExistsById(request.AutorId.Value);
        if (!autorExiste)
            throw new ResourceNotFoundException("Autor não encontrado.");

        livro.Update(request.NomeLivro, request.Preco, request.DataLancamento, request.AutorId.Value);
        livroRepository.Update(livro);

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
            return false;

        livroRepository.Delete(livro);
        return true;
    }
}
