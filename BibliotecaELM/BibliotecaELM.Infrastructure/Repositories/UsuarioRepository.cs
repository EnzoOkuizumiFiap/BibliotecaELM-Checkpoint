using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using BibliotecaELM.Infrastructure.Persistence;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class UsuarioRepository(BibliotecaElmContext bibliotecaElmContext) : IUsuarioRepository
{
    public IReadOnlyList<UsuarioResponse> GetAll()
    {
        return bibliotecaElmContext.Usuarios
            .OrderBy(u => u.Id)
            .Select(UsuarioResponse.FromDomain)
            .ToList();
    }
    
    public UsuarioResponse? GetById(Guid id)
    {
        var usuario = bibliotecaElmContext.Usuarios
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
        var movie = bibliotecaElmContext.Usuarios.FirstOrDefault(u => u.Id == id);
        if (movie is null)
            return false;

        bibliotecaElmContext.Usuarios.Remove(movie);
        bibliotecaElmContext.SaveChanges();

        return true;
    }
}