using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using BibliotecaELM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class UsuarioRepository(BibliotecaElmContext bibliotecaElmContext) : IUsuarioRepository
{
    public IReadOnlyList<UsuarioResponse> GetAll()
    {
        return bibliotecaElmContext.Usuarios
            .Include(u => u.Endereco)
            .OrderBy(u => u.Id)
            .Select(UsuarioResponse.FromDomain)
            .ToList();
    }
    
    public UsuarioResponse? GetById(Guid id)
    {
        var usuario = bibliotecaElmContext.Usuarios
            .Include(u => u.Endereco)
            .FirstOrDefault(u => u.Id == id);

        return usuario is null ? null : UsuarioResponse.FromDomain(usuario);
    }
    
    public UsuarioResponse Create(UsuarioRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new InvalidOperationException("O email do usuario é obrigatório");

        if (ExistsByEmail(request.Email))
            throw new InvalidOperationException("Já existe um usuario com este email");

        var usuario = request.ToDomain();

        bibliotecaElmContext.Usuarios.Add(usuario);
        bibliotecaElmContext.SaveChanges();

        return UsuarioResponse.FromDomain(usuario);
    }

    public UsuarioResponse? Update(Guid id, UsuarioRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
            throw new InvalidOperationException("O Id do usuario é obrigatório");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new InvalidOperationException("O email do usuario é obrigatório");

        if (request.Endereco is null)
            throw new InvalidOperationException("O endereço do usuario é obrigatório");

        var normalizedEmail = request.Email.Trim().ToLower();
        var emailEmUsoPorOutroUsuario = bibliotecaElmContext.Usuarios
            .FirstOrDefault(u => u.Id != id && u.Email.ToLower() == normalizedEmail) is not null;

        if (emailEmUsoPorOutroUsuario)
            throw new InvalidOperationException("Já existe um usuario com este email");

        var usuario = bibliotecaElmContext.Usuarios
            .Include(u => u.Endereco)
            .FirstOrDefault(u => u.Id == id);

        if (usuario is null)
            return null;

        var usuarioEntry = bibliotecaElmContext.Entry(usuario);
        usuarioEntry.Property(u => u.NomeUsuario).CurrentValue = request.NomeUsuario;
        usuarioEntry.Property(u => u.Nascimento).CurrentValue = request.Nascimento;
        usuarioEntry.Property(u => u.Email).CurrentValue = request.Email;
        usuarioEntry.Property(u => u.Cpf).CurrentValue = request.Cpf;

        var enderecoRequest = request.Endereco;
        if (usuario.Endereco is null)
        {
            var novoEndereco = new Domain.Entities.Endereco(
                enderecoRequest.Cep,
                enderecoRequest.Estado,
                enderecoRequest.Cidade,
                enderecoRequest.Bairro,
                enderecoRequest.Rua,
                id);

            bibliotecaElmContext.Enderecos.Add(novoEndereco);
        }
        else
        {
            var enderecoEntry = bibliotecaElmContext.Entry(usuario.Endereco);
            enderecoEntry.Property(e => e.Cep).CurrentValue = enderecoRequest.Cep;
            enderecoEntry.Property(e => e.Estado).CurrentValue = enderecoRequest.Estado;
            enderecoEntry.Property(e => e.Cidade).CurrentValue = enderecoRequest.Cidade;
            enderecoEntry.Property(e => e.Bairro).CurrentValue = enderecoRequest.Bairro;
            enderecoEntry.Property(e => e.Rua).CurrentValue = enderecoRequest.Rua;
            enderecoEntry.Property(e => e.UsuarioId).CurrentValue = id;
        }

        bibliotecaElmContext.SaveChanges();

        var usuarioAtualizado = bibliotecaElmContext.Usuarios
            .Include(u => u.Endereco)
            .FirstOrDefault(u => u.Id == id);

        return usuarioAtualizado is null ? null : UsuarioResponse.FromDomain(usuarioAtualizado);
    }
    
    public bool ExistsByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var normalizedTitle = email.Trim().ToLower();
        var usuario = bibliotecaElmContext.Usuarios
            .FirstOrDefault(u => u.Email.ToLower() == normalizedTitle);

        return usuario is not null;
    }
    
    public bool ExistsById(Guid id)
    {
        var usuario = bibliotecaElmContext.Usuarios
            .FirstOrDefault(u => u.Id == id);

        return usuario is not null;
    }
    
    public bool Delete(Guid id)
    {
        var usuario = bibliotecaElmContext.Usuarios.FirstOrDefault(u => u.Id == id);
        if (usuario is null)
            return false;

        bibliotecaElmContext.Usuarios.Remove(usuario);
        bibliotecaElmContext.SaveChanges();

        return true;
    }
}