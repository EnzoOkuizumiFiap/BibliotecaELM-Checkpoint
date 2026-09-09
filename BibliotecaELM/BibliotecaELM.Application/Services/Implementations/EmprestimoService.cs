using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BibliotecaELM.Application.Services.Implementations;

public sealed class EmprestimoService(
    IEmprestimoRepository emprestimoRepository,
    IUsuarioRepository usuarioRepository,
    ILivroRepository livroRepository,
    ILogger<EmprestimoService>? logger = null) : IEmprestimoService
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
        {
            logger?.LogWarning("Tentativa de empréstimo falhou: UsuarioId ausente.");
            throw new BusinessRuleValidationException("O UsuarioId do empréstimo é obrigatório.");
        }

        if (request.DataEmprestimo == null || request.DataEmprestimo.Value == default)
        {
            logger?.LogWarning("Tentativa de empréstimo falhou: Data de empréstimo ausente.");
            throw new BusinessRuleValidationException("A data de empréstimo é obrigatória.");
        }

        if (request.DataDevolucao == null || request.DataDevolucao.Value == default)
        {
            logger?.LogWarning("Tentativa de empréstimo falhou: Data de devolução ausente.");
            throw new BusinessRuleValidationException("A data de devolução é obrigatória.");
        }

        if (request.DataDevolucao.Value > DateTime.Now)
        {
            logger?.LogWarning("Tentativa de empréstimo falhou: Data de devolução futura.");
            throw new BusinessRuleValidationException("A data de devolução não pode ser futura.");
        }

        if (request.LivrosIds is null || request.LivrosIds.Count == 0)
        {
            logger?.LogWarning("Tentativa de empréstimo falhou: Nenhum livro informado.");
            throw new BusinessRuleValidationException("Ao menos um livro é obrigatório no empréstimo.");
        }

        var usuarioExiste = usuarioRepository.ExistsById(request.UsuarioId.Value);
        if (!usuarioExiste)
        {
            logger?.LogWarning("Tentativa de empréstimo falhou: Usuário {UsuarioId} não encontrado.", request.UsuarioId.Value);
            throw new ResourceNotFoundException("Usuário não encontrado.");
        }

        var livroIds = request.LivrosIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (livroIds.Count == 0)
        {
            logger?.LogWarning("Tentativa de empréstimo falhou: Livros inválidos.");
            throw new BusinessRuleValidationException("Os livros do empréstimo são inválidos.");
        }

        var livros = new List<Livro>();
        foreach (var id in livroIds)
        {
            var livro = livroRepository.GetById(id);
            if (livro is null)
            {
                logger?.LogWarning("Tentativa de empréstimo falhou: Livro {LivroId} não encontrado.", id);
                throw new ResourceNotFoundException($"Livro com ID {id} não encontrado.");
            }
            livros.Add(livro);
        }

        var emprestimoDomain = request.ToDomain(livros);
        emprestimoRepository.Add(emprestimoDomain);

        logger?.LogInformation("Empréstimo {EmprestimoId} criado com sucesso no repositório para o usuário {UsuarioId}.", emprestimoDomain.Id, emprestimoDomain.UsuarioId);

        return EmprestimoResponse.FromDomain(emprestimoDomain);
    }

    public EmprestimoResponse? Update(Guid id, EmprestimoRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
        {
            logger?.LogWarning("Tentativa de atualização de empréstimo falhou: Id vazio.");
            throw new BusinessRuleValidationException("O Id do empréstimo é obrigatório.");
        }

        if (request.UsuarioId == null || request.UsuarioId.Value == Guid.Empty)
        {
            logger?.LogWarning("Tentativa de atualização de empréstimo {EmprestimoId} falhou: UsuarioId inválido.", id);
            throw new BusinessRuleValidationException("O UsuarioId do empréstimo é obrigatório.");
        }

        if (request.DataEmprestimo == null || request.DataEmprestimo.Value == default)
        {
            logger?.LogWarning("Tentativa de atualização de empréstimo {EmprestimoId} falhou: Data empréstimo inválida.", id);
            throw new BusinessRuleValidationException("A data de empréstimo é obrigatória.");
        }

        if (request.DataDevolucao == null || request.DataDevolucao.Value == default)
        {
            logger?.LogWarning("Tentativa de atualização de empréstimo {EmprestimoId} falhou: Data devolução inválida.", id);
            throw new BusinessRuleValidationException("A data de devolução é obrigatória.");
        }

        if (request.LivrosIds is null || request.LivrosIds.Count == 0)
        {
            logger?.LogWarning("Tentativa de atualização de empréstimo {EmprestimoId} falhou: Nenhum livro.", id);
            throw new BusinessRuleValidationException("Ao menos um livro é obrigatório no empréstimo.");
        }

        var emprestimo = emprestimoRepository.GetById(id);
        if (emprestimo is null)
        {
            logger?.LogWarning("Tentativa de atualização falhou: Empréstimo {EmprestimoId} não encontrado.", id);
            return null;
        }

        var usuarioExiste = usuarioRepository.ExistsById(request.UsuarioId.Value);
        if (!usuarioExiste)
        {
            logger?.LogWarning("Tentativa de atualização de empréstimo falhou: Usuário {UsuarioId} não encontrado.", request.UsuarioId.Value);
            throw new ResourceNotFoundException("Usuário não encontrado.");
        }

        var livroIds = request.LivrosIds
            .Where(livroId => livroId != Guid.Empty)
            .Distinct()
            .ToList();

        if (livroIds.Count == 0)
        {
            logger?.LogWarning("Tentativa de atualização de empréstimo falhou: Livros inválidos.");
            throw new BusinessRuleValidationException("Os livros do empréstimo são inválidos.");
        }

        var livros = new List<Livro>();
        foreach (var lId in livroIds)
        {
            var livro = livroRepository.GetById(lId);
            if (livro is null)
            {
                logger?.LogWarning("Tentativa de atualização de empréstimo falhou: Livro {LivroId} não encontrado.", lId);
                throw new ResourceNotFoundException($"Livro com ID {lId} não encontrado.");
            }
            livros.Add(livro);
        }

        emprestimo.Update(request.DataEmprestimo.Value, request.DataDevolucao.Value, request.UsuarioId.Value, livros);
        emprestimoRepository.Update(emprestimo);

        logger?.LogInformation("Empréstimo {EmprestimoId} atualizado com sucesso no repositório.", emprestimo.Id);

        return EmprestimoResponse.FromDomain(emprestimo);
    }

    public bool Delete(Guid id)
    {
        var emprestimo = emprestimoRepository.GetById(id);
        if (emprestimo is null)
        {
            logger?.LogWarning("Tentativa de exclusão falhou: Empréstimo {EmprestimoId} não encontrado.", id);
            return false;
        }

        emprestimoRepository.Delete(emprestimo);
        logger?.LogInformation("Empréstimo {EmprestimoId} removido com sucesso do repositório.", id);
        return true;
    }
}
