using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using BibliotecaELM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class EmprestimoRepository(BibliotecaElmContext bibliotecaElmContext) : IEmprestimoRepository
{
	public IReadOnlyList<EmprestimoResponse> GetAll()
	{
		return bibliotecaElmContext.Emprestimos
			.Include(e => e.Livros)
			.OrderBy(e => e.Id)
			.Select(EmprestimoResponse.FromDomain)
			.ToList();
	}

	public EmprestimoResponse? GetById(Guid id)
	{
		var emprestimo = bibliotecaElmContext.Emprestimos
			.Include(e => e.Livros)
			.FirstOrDefault(e => e.Id == id);

		return emprestimo is null ? null : EmprestimoResponse.FromDomain(emprestimo);
	}

	public EmprestimoResponse Create(EmprestimoRequest request)
	{
		if (request is null)
			throw new ArgumentNullException(nameof(request));

		if (request.UsuarioId == Guid.Empty)
			throw new InvalidOperationException("O UsuarioId do empréstimo é obrigatório");

		if (request.DataEmprestimo == default)
			throw new InvalidOperationException("A data de empréstimo é obrigatória");

		if (request.DataDevolucao == default)
			throw new InvalidOperationException("A data de devolução é obrigatória");

		if (request.DataDevolucao < request.DataEmprestimo)
			throw new InvalidOperationException("A data de devolução não pode ser menor que a data de empréstimo");

		if (request.LivrosIds is null || request.LivrosIds.Count == 0)
			throw new InvalidOperationException("Ao menos um livro é obrigatório no empréstimo");

		var usuarioExiste = bibliotecaElmContext.Usuarios
			.FirstOrDefault(u => u.Id == request.UsuarioId) is not null;

		if (!usuarioExiste)
			throw new InvalidOperationException("Usuário não encontrado");

		var livroIds = request.LivrosIds
			.Where(id => id != Guid.Empty)
			.Distinct()
			.ToList();

		if (livroIds.Count == 0)
			throw new InvalidOperationException("Os livros do empréstimo são inválidos");

		var livros = bibliotecaElmContext.Livros
			.Where(l => livroIds.Contains(l.Id))
			.ToList();

		if (livros.Count != livroIds.Count)
			throw new InvalidOperationException("Um ou mais livros não foram encontrados");

		var emprestimoDomain = request.ToDomain(livros);

		bibliotecaElmContext.Emprestimos.Add(emprestimoDomain);
		bibliotecaElmContext.SaveChanges();

		return EmprestimoResponse.FromDomain(emprestimoDomain);
	}

	public EmprestimoResponse? Update(Guid id, EmprestimoRequest request)
	{
		if (request is null)
			throw new ArgumentNullException(nameof(request));

		if (id == Guid.Empty)
			throw new InvalidOperationException("O Id do empréstimo é obrigatório");

		if (request.UsuarioId == Guid.Empty)
			throw new InvalidOperationException("O UsuarioId do empréstimo é obrigatório");

		if (request.DataEmprestimo == default)
			throw new InvalidOperationException("A data de empréstimo é obrigatória");

		if (request.DataDevolucao == default)
			throw new InvalidOperationException("A data de devolução é obrigatória");

		if (request.DataDevolucao < request.DataEmprestimo)
			throw new InvalidOperationException("A data de devolução não pode ser menor que a data de empréstimo");

		if (request.LivrosIds is null || request.LivrosIds.Count == 0)
			throw new InvalidOperationException("Ao menos um livro é obrigatório no empréstimo");

		var emprestimo = bibliotecaElmContext.Emprestimos
			.Include(e => e.Livros)
			.FirstOrDefault(e => e.Id == id);

		if (emprestimo is null)
			return null;

		var usuarioExiste = bibliotecaElmContext.Usuarios
			.FirstOrDefault(u => u.Id == request.UsuarioId) is not null;

		if (!usuarioExiste)
			throw new InvalidOperationException("Usuário não encontrado");

		var livroIds = request.LivrosIds
			.Where(livroId => livroId != Guid.Empty)
			.Distinct()
			.ToList();

		if (livroIds.Count == 0)
			throw new InvalidOperationException("Os livros do empréstimo são inválidos");

		var livros = bibliotecaElmContext.Livros
			.Where(l => livroIds.Contains(l.Id))
			.ToList();

		if (livros.Count != livroIds.Count)
			throw new InvalidOperationException("Um ou mais livros não foram encontrados");

		var emprestimoEntry = bibliotecaElmContext.Entry(emprestimo);
		emprestimoEntry.Property(e => e.DataEmprestimo).CurrentValue = request.DataEmprestimo;
		emprestimoEntry.Property(e => e.DataDevolucao).CurrentValue = request.DataDevolucao;
		emprestimoEntry.Property(e => e.UsuarioId).CurrentValue = request.UsuarioId;

		if (emprestimo.Livros is null)
		{
			emprestimoEntry.Collection(e => e.Livros).CurrentValue = new List<Domain.Entities.Livro>();
		}

		var livrosDoEmprestimo = emprestimo.Livros ?? new List<Domain.Entities.Livro>();
		livrosDoEmprestimo.Clear();
		foreach (var livro in livros)
		{
			livrosDoEmprestimo.Add(livro);
		}

		emprestimoEntry.Collection(e => e.Livros).CurrentValue = livrosDoEmprestimo;

		bibliotecaElmContext.SaveChanges();

		return EmprestimoResponse.FromDomain(emprestimo);
	}

	public bool ExistsById(Guid id)
	{
		return bibliotecaElmContext.Emprestimos.FirstOrDefault(e => e.Id == id) is not null;
	}

	public bool Delete(Guid id)
	{
		var emprestimo = bibliotecaElmContext.Emprestimos.FirstOrDefault(e => e.Id == id);
		if (emprestimo is null)
			return false;

		bibliotecaElmContext.Emprestimos.Remove(emprestimo);
		bibliotecaElmContext.SaveChanges();

		return true;
	}
}