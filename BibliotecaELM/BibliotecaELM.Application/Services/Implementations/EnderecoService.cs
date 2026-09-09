using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BibliotecaELM.Application.Services.Implementations;

public sealed class EnderecoService(
    IEnderecoRepository enderecoRepository,
    IUsuarioRepository usuarioRepository,
    ILogger<EnderecoService>? logger = null) : IEnderecoService
{
    public IReadOnlyList<EnderecoResponse> GetAll()
    {
        return enderecoRepository.GetAll()
            .OrderBy(e => e.Id)
            .Select(EnderecoResponse.FromDomain)
            .ToList();
    }

    public EnderecoResponse? GetById(Guid id)
    {
        var endereco = enderecoRepository.GetById(id);
        return endereco is null ? null : EnderecoResponse.FromDomain(endereco);
    }

    public EnderecoResponse Create(EnderecoRequest request, Guid usuarioId)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (usuarioId == Guid.Empty)
        {
            logger?.LogWarning("Tentativa de cadastro de endereço falhou: UsuarioId ausente.");
            throw new BusinessRuleValidationException("O ID do usuário é obrigatório.");
        }

        var usuarioExiste = usuarioRepository.ExistsById(usuarioId);
        if (!usuarioExiste)
        {
            logger?.LogWarning("Tentativa de cadastro de endereço falhou: Usuário {UsuarioId} não encontrado.", usuarioId);
            throw new ResourceNotFoundException("Usuário não encontrado.");
        }

        var jaPossuiEndereco = enderecoRepository.ExistsByIdUsuario(usuarioId);
        if (jaPossuiEndereco)
        {
            logger?.LogWarning("Tentativa de cadastro de endereço falhou: Usuário {UsuarioId} já possui endereço cadastrado.", usuarioId);
            throw new BusinessRuleValidationException("O usuário informado já possui um endereço cadastrado.");
        }

        var endereco = request.ToDomain(usuarioId);
        enderecoRepository.Add(endereco);

        logger?.LogInformation("Endereço {EnderecoId} cadastrado com sucesso no repositório para o usuário {UsuarioId}.", endereco.Id, usuarioId);

        return EnderecoResponse.FromDomain(endereco);
    }

    public EnderecoResponse? Update(Guid id, EnderecoRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
        {
            logger?.LogWarning("Tentativa de atualização de endereço falhou: Id vazio.");
            throw new BusinessRuleValidationException("O ID do endereço é obrigatório.");
        }

        var endereco = enderecoRepository.GetById(id);
        if (endereco is null)
        {
            logger?.LogWarning("Tentativa de atualização falhou: Endereço {EnderecoId} não encontrado.", id);
            return null;
        }

        endereco.Update(request.Cep, request.Estado, request.Cidade, request.Bairro, request.Rua, endereco.UsuarioId);
        enderecoRepository.Update(endereco);

        logger?.LogInformation("Endereço {EnderecoId} atualizado com sucesso no repositório.", endereco.Id);

        return EnderecoResponse.FromDomain(endereco);
    }

    public bool Delete(Guid id)
    {
        var endereco = enderecoRepository.GetById(id);
        if (endereco is null)
        {
            logger?.LogWarning("Tentativa de exclusão falhou: Endereço {EnderecoId} não encontrado.", id);
            return false;
        }

        enderecoRepository.Delete(endereco);
        logger?.LogInformation("Endereço {EnderecoId} removido com sucesso do repositório.", id);
        return true;
    }
}
