using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;

using BibliotecaELM.Application.Services.Interfaces;

namespace BibliotecaELM.Application.Services.Implementations;

public sealed class EnderecoService(
    IEnderecoRepository enderecoRepository,
    IUsuarioRepository usuarioRepository) : IEnderecoService
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
            throw new BusinessRuleValidationException("O ID do usuário é obrigatório.");

        var usuarioExiste = usuarioRepository.ExistsById(usuarioId);
        if (!usuarioExiste)
            throw new ResourceNotFoundException("Usuário não encontrado.");

        var jaPossuiEndereco = enderecoRepository.ExistsByIdUsuario(usuarioId);
        if (jaPossuiEndereco)
            throw new BusinessRuleValidationException("O usuário informado já possui um endereço cadastrado.");

        var endereco = request.ToDomain(usuarioId);
        enderecoRepository.Add(endereco);

        return EnderecoResponse.FromDomain(endereco);
    }

    public EnderecoResponse? Update(Guid id, EnderecoRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
            throw new BusinessRuleValidationException("O ID do endereço é obrigatório.");

        var endereco = enderecoRepository.GetById(id);
        if (endereco is null)
            return null;

        endereco.Update(request.Cep, request.Estado, request.Cidade, request.Bairro, request.Rua, endereco.UsuarioId);
        enderecoRepository.Update(endereco);

        return EnderecoResponse.FromDomain(endereco);
    }

    public bool Delete(Guid id)
    {
        var endereco = enderecoRepository.GetById(id);
        if (endereco is null)
            return false;

        enderecoRepository.Delete(endereco);
        return true;
    }
}
