using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;

using BibliotecaELM.Application.Services.Interfaces;

namespace BibliotecaELM.Application.Services.Implementations;

public sealed class CompraService(
    ICompraRepository compraRepository,
    IUsuarioRepository usuarioRepository,
    IEnderecoRepository enderecoRepository,
    ILivroRepository livroRepository) : ICompraService
{
    public IReadOnlyList<CompraResponse> GetAll()
    {
        return compraRepository.GetAll()
            .OrderBy(c => c.Id)
            .Select(CompraResponse.FromDomain)
            .ToList();
    }

    public CompraResponse? GetById(Guid id)
    {
        var compra = compraRepository.GetById(id);
        return compra is null ? null : CompraResponse.FromDomain(compra);
    }

    public CompraResponse Create(CompraRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (request.FormaCompra == null || !Enum.IsDefined(request.FormaCompra.Value))
            throw new BusinessRuleValidationException("O formato do pagamento da compra é obrigatório.");

        if (request.DataCompra == null || request.DataCompra.Value == default)
            throw new BusinessRuleValidationException("A data da compra é obrigatória.");

        if (request.UsuarioId == null || request.UsuarioId.Value == Guid.Empty)
            throw new BusinessRuleValidationException("O UsuarioId da compra é obrigatório.");

        if (request.LivrosIds is null || request.LivrosIds.Count == 0)
            throw new BusinessRuleValidationException("Ao menos um livro é obrigatório na compra.");

        var usuarioExiste = usuarioRepository.ExistsById(request.UsuarioId.Value);
        if (!usuarioExiste)
            throw new ResourceNotFoundException("Usuário não encontrado.");

        var usuarioTemEndereco = enderecoRepository.ExistsByIdUsuario(request.UsuarioId.Value);
        if (!usuarioTemEndereco)
            throw new BusinessRuleValidationException("Usuário sem endereço não pode realizar compra.");

        var livroIds = request.LivrosIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (livroIds.Count == 0)
            throw new BusinessRuleValidationException("Os livros da compra são inválidos.");

        var livros = new List<Livro>();
        foreach (var id in livroIds)
        {
            var livro = livroRepository.GetById(id);
            if (livro is null)
                throw new ResourceNotFoundException($"Livro com ID {id} não encontrado.");
            livros.Add(livro);
        }

        var compraDomain = request.ToDomain(livros);
        compraRepository.Add(compraDomain);

        return CompraResponse.FromDomain(compraDomain);
    }

    public CompraResponse? Update(Guid id, CompraRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
            throw new BusinessRuleValidationException("O Id da compra é obrigatório.");

        if (request.FormaCompra == null || !Enum.IsDefined(request.FormaCompra.Value))
            throw new BusinessRuleValidationException("O formato do pagamento da compra é obrigatório.");

        if (request.DataCompra == null || request.DataCompra.Value == default)
            throw new BusinessRuleValidationException("A data da compra é obrigatória.");

        if (request.UsuarioId == null || request.UsuarioId.Value == Guid.Empty)
            throw new BusinessRuleValidationException("O UsuarioId da compra é obrigatório.");

        if (request.LivrosIds is null || request.LivrosIds.Count == 0)
            throw new BusinessRuleValidationException("Ao menos um livro é obrigatório na compra.");

        var compra = compraRepository.GetById(id);
        if (compra is null)
            return null;

        var usuarioExiste = usuarioRepository.ExistsById(request.UsuarioId.Value);
        if (!usuarioExiste)
            throw new ResourceNotFoundException("Usuário não encontrado.");

        var usuarioTemEndereco = enderecoRepository.ExistsByIdUsuario(request.UsuarioId.Value);
        if (!usuarioTemEndereco)
            throw new BusinessRuleValidationException("Usuário sem endereço não pode realizar compra.");

        var livroIds = request.LivrosIds
            .Where(livroId => livroId != Guid.Empty)
            .Distinct()
            .ToList();

        if (livroIds.Count == 0)
            throw new BusinessRuleValidationException("Os livros da compra são inválidos.");

        var livros = new List<Livro>();
        foreach (var lId in livroIds)
        {
            var livro = livroRepository.GetById(lId);
            if (livro is null)
                throw new ResourceNotFoundException($"Livro com ID {lId} não encontrado.");
            livros.Add(livro);
        }

        compra.Update(request.FormaCompra.Value, request.DataCompra.Value, request.UsuarioId.Value, livros);
        compraRepository.Update(compra);

        return CompraResponse.FromDomain(compra);
    }

    public bool Delete(Guid id)
    {
        var compra = compraRepository.GetById(id);
        if (compra is null)
            return false;

        compraRepository.Delete(compra);
        return true;
    }
}
