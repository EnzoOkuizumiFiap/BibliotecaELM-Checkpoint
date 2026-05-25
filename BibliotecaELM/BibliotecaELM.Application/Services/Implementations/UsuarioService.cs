using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Domain.Exceptions;

using BibliotecaELM.Application.Services.Interfaces;

namespace BibliotecaELM.Application.Services.Implementations;

public sealed class UsuarioService(IUsuarioRepository usuarioRepository) : IUsuarioService
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
            throw new BusinessRuleValidationException("O E-mail do usuário é obrigatório.");

        if (usuarioRepository.ExistsByEmail(request.Email))
            throw new BusinessRuleValidationException("Já existe um usuário cadastrado com este e-mail.");

        var usuario = request.ToDomain();
        usuarioRepository.Add(usuario);

        return UsuarioResponse.FromDomain(usuario);
    }

    public UsuarioResponse? Update(Guid id, UsuarioRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
            throw new BusinessRuleValidationException("O Id do usuário é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new BusinessRuleValidationException("O E-mail do usuário é obrigatório.");

        var usuario = usuarioRepository.GetById(id);
        if (usuario is null)
            return null;

        var normalizedEmail = request.Email.Trim().ToLower();
        var usuarioComEmail = usuarioRepository.GetAll().FirstOrDefault(u => u.Email.Trim().ToLower() == normalizedEmail);
        if (usuarioComEmail != null && usuarioComEmail.Id != id)
            throw new BusinessRuleValidationException("Já existe um usuário cadastrado com este e-mail.");

        usuario.Update(request.NomeUsuario, request.Nascimento, request.Email, request.Cpf);
        usuarioRepository.Update(usuario);

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
            return false;

        usuarioRepository.Delete(usuario);
        return true;
    }
}
