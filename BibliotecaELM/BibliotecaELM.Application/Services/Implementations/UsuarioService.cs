using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BibliotecaELM.Application.Services.Implementations;

public sealed class UsuarioService(
    IUsuarioRepository usuarioRepository,
    ILogger<UsuarioService>? logger = null) : IUsuarioService
{
    public IReadOnlyList<UsuarioResponse> GetAll()
    {
        return usuarioRepository.GetAll()
            .OrderBy(u => u.Id)
            .Select(UsuarioResponse.FromDomain)
            .ToList();
    }

    public UsuarioResponse? GetById(Guid id)
    {
        var usuario = usuarioRepository.GetById(id);
        return usuario is null ? null : UsuarioResponse.FromDomain(usuario);
    }

    public UsuarioResponse Create(UsuarioRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            logger?.LogWarning("Tentativa de criação de usuário falhou: E-mail obrigatório.");
            throw new BusinessRuleValidationException("O E-mail do usuário é obrigatório.");
        }

        if (usuarioRepository.ExistsByEmail(request.Email))
        {
            logger?.LogWarning("Tentativa de criação de usuário falhou: Email {Email} já cadastrado.", request.Email);
            throw new BusinessRuleValidationException("Já existe um usuário cadastrado com este e-mail.");
        }

        var usuario = request.ToDomain();
        usuarioRepository.Add(usuario);

        logger?.LogInformation("Usuário {UsuarioId} ({Email}) criado com sucesso no repositório.", usuario.Id, usuario.Email);

        return UsuarioResponse.FromDomain(usuario);
    }

    public UsuarioResponse? Update(Guid id, UsuarioRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
        {
            logger?.LogWarning("Tentativa de atualização de usuário falhou: Id vazio.");
            throw new BusinessRuleValidationException("O Id do usuário é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            logger?.LogWarning("Tentativa de atualização do usuário {UsuarioId} falhou: E-mail vazio.", id);
            throw new BusinessRuleValidationException("O E-mail do usuário é obrigatório.");
        }

        var usuario = usuarioRepository.GetById(id);
        if (usuario is null)
        {
            logger?.LogWarning("Tentativa de atualização falhou: Usuário {UsuarioId} não encontrado.", id);
            return null;
        }

        var normalizedEmail = request.Email.Trim().ToLower();
        var usuarioComEmail = usuarioRepository.GetAll().FirstOrDefault(u => u.Email.Trim().ToLower() == normalizedEmail);
        if (usuarioComEmail != null && usuarioComEmail.Id != id)
        {
            logger?.LogWarning("Tentativa de atualização do usuário {UsuarioId} falhou: Email {Email} já utilizado.", id, request.Email);
            throw new BusinessRuleValidationException("Já existe um usuário cadastrado com este e-mail.");
        }

        usuario.Update(request.NomeUsuario, request.Nascimento, request.Email, request.Cpf);
        usuarioRepository.Update(usuario);

        logger?.LogInformation("Usuário {UsuarioId} ({Email}) atualizado com sucesso no repositório.", usuario.Id, usuario.Email);

        return UsuarioResponse.FromDomain(usuario);
    }

    public bool ExistsByEmail(string email)
    {
        return usuarioRepository.ExistsByEmail(email);
    }

    public bool Delete(Guid id)
    {
        var usuario = usuarioRepository.GetById(id);
        if (usuario is null)
        {
            logger?.LogWarning("Tentativa de exclusão falhou: Usuário {UsuarioId} não encontrado.", id);
            return false;
        }

        usuarioRepository.Delete(usuario);
        logger?.LogInformation("Usuário {UsuarioId} removido com sucesso do repositório.", id);
        return true;
    }
}
