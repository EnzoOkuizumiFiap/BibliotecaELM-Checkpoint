using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;

using BibliotecaELM.Application.Services.Interfaces;

namespace BibliotecaELM.Application.Services.Implementations;

public sealed class EmprestimoService(
    IEmprestimoRepository emprestimoRepository,
    IUsuarioRepository usuarioRepository,
    ILivroRepository livroRepository) : IEmprestimoService
{
    public IReadOnlyList<EmprestimoResponse> GetAll()
    {
        return emprestimoRepository.GetAll()
            .OrderBy(e => e.Id)
            .Select(EmprestimoResponse.FromDomain)
            .ToList();
    }

    public EmprestimoResponse? GetById(Guid id)
    {
        var emprestimo = emprestimoRepository.GetById(id);
        return emprestimo is null ? null : EmprestimoResponse.FromDomain(emprestimo);
    }

    public EmprestimoResponse Create(EmprestimoRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (request.UsuarioId == null || request.UsuarioId.Value == Guid.Empty)
            throw new BusinessRuleValidationException("O UsuarioId do empréstimo é obrigatório.");

        if (request.DataEmprestimo == null || request.DataEmprestimo.Value == default)
            throw new BusinessRuleValidationException("A data de empréstimo é obrigatória.");

        if (request.DataDevolucao == null || request.DataDevolucao.Value == default)
            throw new BusinessRuleValidationException("A data de devolução é obrigatória.");

        if (request.DataDevolucao.Value > DateTime.Now)
            throw new BusinessRuleValidationException("A data de devolução não pode ser futura.");

        if (request.LivrosIds is null || request.LivrosIds.Count == 0)
            throw new BusinessRuleValidationException("Ao menos um livro é obrigatório no empréstimo.");

        var usuarioExiste = usuarioRepository.ExistsById(request.UsuarioId.Value);
        if (!usuarioExiste)
            throw new ResourceNotFoundException("Usuário não encontrado.");

        var livroIds = request.LivrosIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (livroIds.Count == 0)
            throw new BusinessRuleValidationException("Os livros do empréstimo são inválidos.");

        var livros = new List<Livro>();
        foreach (var id in livroIds)
        {
            var livro = livroRepository.GetById(id);
            if (livro is null)
                throw new ResourceNotFoundException($"Livro com ID {id} não encontrado.");
            livros.Add(livro);
        }

        var emprestimoDomain = request.ToDomain(livros);
        emprestimoRepository.Add(emprestimoDomain);

        return EmprestimoResponse.FromDomain(emprestimoDomain);
    }

    public EmprestimoResponse? Update(Guid id, EmprestimoRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
            throw new BusinessRuleValidationException("O Id do empréstimo é obrigatório.");

        if (request.UsuarioId == null || request.UsuarioId.Value == Guid.Empty)
            throw new BusinessRuleValidationException("O UsuarioId do empréstimo é obrigatório.");

        if (request.DataEmprestimo == null || request.DataEmprestimo.Value == default)
            throw new BusinessRuleValidationException("A data de empréstimo é obrigatória.");

        if (request.DataDevolucao == null || request.DataDevolucao.Value == default)
            throw new BusinessRuleValidationException("A data de devolução é obrigatória.");

        if (request.LivrosIds is null || request.LivrosIds.Count == 0)
            throw new BusinessRuleValidationException("Ao menos um livro é obrigatório no empréstimo.");

        var emprestimo = emprestimoRepository.GetById(id);
        if (emprestimo is null)
            return null;

        var usuarioExiste = usuarioRepository.ExistsById(request.UsuarioId.Value);
        if (!usuarioExiste)
            throw new ResourceNotFoundException("Usuário não encontrado.");

        var livroIds = request.LivrosIds
            .Where(livroId => livroId != Guid.Empty)
            .Distinct()
            .ToList();

        if (livroIds.Count == 0)
            throw new BusinessRuleValidationException("Os livros do empréstimo são inválidos.");

        var livros = new List<Livro>();
        foreach (var lId in livroIds)
        {
            var livro = livroRepository.GetById(lId);
            if (livro is null)
                throw new ResourceNotFoundException($"Livro com ID {lId} não encontrado.");
            livros.Add(livro);
        }

        emprestimo.Update(request.DataEmprestimo.Value, request.DataDevolucao.Value, request.UsuarioId.Value, livros);
        emprestimoRepository.Update(emprestimo);

        return EmprestimoResponse.FromDomain(emprestimo);
    }

    public bool Delete(Guid id)
    {
        var emprestimo = emprestimoRepository.GetById(id);
        if (emprestimo is null)
            return false;

        emprestimoRepository.Delete(emprestimo);
        return true;
    }
}
