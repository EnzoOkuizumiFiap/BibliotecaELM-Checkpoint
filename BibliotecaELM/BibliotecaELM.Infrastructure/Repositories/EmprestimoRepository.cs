using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using BibliotecaELM.Infrastructure.Persistence;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class EmprestimoRepository(BibliotecaElmContext bibliotecaElmContext) : IEmprestimoRepository
{
	public IReadOnlyList<EmprestimoResponse> GetAll()
	{
		return bibliotecaElmContext.Emprestimos
			.OrderBy(e => e.Id)
			.Select(EmprestimoResponse.FromDomain)
			.ToList();
	}

	public EmprestimoResponse? GetById(Guid id)
	{
		var emprestimo = bibliotecaElmContext.Emprestimos
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

		if (request.Livros is null || request.Livros.Count == 0)
			throw new InvalidOperationException("Ao menos um livro é obrigatório no empréstimo");

		var usuarioExiste = bibliotecaElmContext.Usuarios
			.Any(u => u.Id == request.UsuarioId);

		if (!usuarioExiste)
			throw new InvalidOperationException("Usuário não encontrado");

		var livroIds = request.Livros
			.Select(l => l.Id)
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

		var emprestimo = request with { Livros = livros };
		var emprestimoDomain = emprestimo.ToDomain();

		bibliotecaElmContext.Emprestimos.Add(emprestimoDomain);
		bibliotecaElmContext.SaveChanges();

		return EmprestimoResponse.FromDomain(emprestimoDomain);
	}

	public bool ExistsById(Guid id)
	{
		return bibliotecaElmContext.Emprestimos.Any(e => e.Id == id);
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