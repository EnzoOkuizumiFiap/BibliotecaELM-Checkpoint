using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BibliotecaELM.Application.Services.Implementations;

public sealed class CompraService(
    ICompraRepository compraRepository,
    IUsuarioRepository usuarioRepository,
    IEnderecoRepository enderecoRepository,
    ILivroRepository livroRepository,
    ILogger<CompraService>? logger = null) : ICompraService
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
        {
            logger?.LogWarning("Tentativa de compra falhou: Forma de compra inválida.");
            throw new BusinessRuleValidationException("O formato do pagamento da compra é obrigatório.");
        }

        if (request.DataCompra == null || request.DataCompra.Value == default)
        {
            logger?.LogWarning("Tentativa de compra falhou: Data da compra ausente.");
            throw new BusinessRuleValidationException("A data da compra é obrigatória.");
        }

        if (request.UsuarioId == null || request.UsuarioId.Value == Guid.Empty)
        {
            logger?.LogWarning("Tentativa de compra falhou: UsuarioId ausente.");
            throw new BusinessRuleValidationException("O UsuarioId da compra é obrigatório.");
        }

        if (request.LivrosIds is null || request.LivrosIds.Count == 0)
        {
            logger?.LogWarning("Tentativa de compra falhou: Nenhum livro informado.");
            throw new BusinessRuleValidationException("Ao menos um livro é obrigatório na compra.");
        }

        var usuarioExiste = usuarioRepository.ExistsById(request.UsuarioId.Value);
        if (!usuarioExiste)
        {
            logger?.LogWarning("Tentativa de compra falhou: Usuário {UsuarioId} não encontrado.", request.UsuarioId.Value);
            throw new ResourceNotFoundException("Usuário não encontrado.");
        }

        var usuarioTemEndereco = enderecoRepository.ExistsByIdUsuario(request.UsuarioId.Value);
        if (!usuarioTemEndereco)
        {
            logger?.LogWarning("Tentativa de compra falhou: Usuário {UsuarioId} não possui endereço cadastrado.", request.UsuarioId.Value);
            throw new BusinessRuleValidationException("Usuário sem endereço não pode realizar compra.");
        }

        var livroIds = request.LivrosIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (livroIds.Count == 0)
        {
            logger?.LogWarning("Tentativa de compra falhou: Livros inválidos.");
            throw new BusinessRuleValidationException("Os livros da compra são inválidos.");
        }

        var livros = new List<Livro>();
        foreach (var id in livroIds)
        {
            var livro = livroRepository.GetById(id);
            if (livro is null)
            {
                logger?.LogWarning("Tentativa de compra falhou: Livro {LivroId} não encontrado.", id);
                throw new ResourceNotFoundException($"Livro com ID {id} não encontrado.");
            }
            livros.Add(livro);
        }

        var compraDomain = request.ToDomain(livros);
        compraRepository.Add(compraDomain);

        logger?.LogInformation("Compra {CompraId} criada com sucesso no repositório para o usuário {UsuarioId}.", compraDomain.Id, compraDomain.UsuarioId);

        return CompraResponse.FromDomain(compraDomain);
    }

    public CompraResponse? Update(Guid id, CompraRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
        {
            logger?.LogWarning("Tentativa de atualização de compra falhou: Id vazio.");
            throw new BusinessRuleValidationException("O Id da compra é obrigatório.");
        }

        if (request.FormaCompra == null || !Enum.IsDefined(request.FormaCompra.Value))
        {
            logger?.LogWarning("Tentativa de atualização de compra {CompraId} falhou: Forma de compra inválida.", id);
            throw new BusinessRuleValidationException("O formato do pagamento da compra é obrigatório.");
        }

        if (request.DataCompra == null || request.DataCompra.Value == default)
        {
            logger?.LogWarning("Tentativa de atualização de compra {CompraId} falhou: Data inválida.", id);
            throw new BusinessRuleValidationException("A data da compra é obrigatória.");
        }

        if (request.UsuarioId == null || request.UsuarioId.Value == Guid.Empty)
        {
            logger?.LogWarning("Tentativa de atualização de compra {CompraId} falhou: UsuarioId inválido.", id);
            throw new BusinessRuleValidationException("O UsuarioId da compra é obrigatório.");
        }

        if (request.LivrosIds is null || request.LivrosIds.Count == 0)
        {
            logger?.LogWarning("Tentativa de atualização de compra {CompraId} falhou: Nenhum livro.", id);
            throw new BusinessRuleValidationException("Ao menos um livro é obrigatório na compra.");
        }

        var compra = compraRepository.GetById(id);
        if (compra is null)
        {
            logger?.LogWarning("Tentativa de atualização falhou: Compra {CompraId} não encontrada.", id);
            return null;
        }

        var usuarioExiste = usuarioRepository.ExistsById(request.UsuarioId.Value);
        if (!usuarioExiste)
        {
            logger?.LogWarning("Tentativa de atualização de compra falhou: Usuário {UsuarioId} não encontrado.", request.UsuarioId.Value);
            throw new ResourceNotFoundException("Usuário não encontrado.");
        }

        var usuarioTemEndereco = enderecoRepository.ExistsByIdUsuario(request.UsuarioId.Value);
        if (!usuarioTemEndereco)
        {
            logger?.LogWarning("Tentativa de atualização de compra falhou: Usuário {UsuarioId} sem endereço.", request.UsuarioId.Value);
            throw new BusinessRuleValidationException("Usuário sem endereço não pode realizar compra.");
        }

        var livroIds = request.LivrosIds
            .Where(livroId => livroId != Guid.Empty)
            .Distinct()
            .ToList();

        if (livroIds.Count == 0)
        {
            logger?.LogWarning("Tentativa de atualização de compra falhou: Livros inválidos.");
            throw new BusinessRuleValidationException("Os livros da compra são inválidos.");
        }

        var livros = new List<Livro>();
        foreach (var lId in livroIds)
        {
            var livro = livroRepository.GetById(lId);
            if (livro is null)
            {
                logger?.LogWarning("Tentativa de atualização de compra falhou: Livro {LivroId} não encontrado.", lId);
                throw new ResourceNotFoundException($"Livro com ID {lId} não encontrado.");
            }
            livros.Add(livro);
        }

        compra.Update(request.FormaCompra.Value, request.DataCompra.Value, request.UsuarioId.Value, livros);
        compraRepository.Update(compra);

        logger?.LogInformation("Compra {CompraId} atualizada com sucesso no repositório.", compra.Id);

        return CompraResponse.FromDomain(compra);
    }

    public bool Delete(Guid id)
    {
        var compra = compraRepository.GetById(id);
        if (compra is null)
        {
            logger?.LogWarning("Tentativa de exclusão falhou: Compra {CompraId} não encontrada.", id);
            return false;
        }

        compraRepository.Delete(compra);
        logger?.LogInformation("Compra {CompraId} removida com sucesso do repositório.", id);
        return true;
    }
}
