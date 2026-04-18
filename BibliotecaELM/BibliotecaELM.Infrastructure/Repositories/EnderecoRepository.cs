using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using BibliotecaELM.Infrastructure.Persistence;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class EnderecoRepository(BibliotecaElmContext bibliotecaElmContext) : IEnderecoRepository
{
    public IReadOnlyList<EnderecoResponse> GetAll()
    {
        return bibliotecaElmContext.Enderecos
            .OrderBy(f => f.Id)
            .Select(EnderecoResponse.FromDomain)
            .ToList();
    }
    
    public EnderecoResponse? GetById(Guid id)
    {
        var endereco = bibliotecaElmContext.Enderecos
            .FirstOrDefault(f => f.Id == id);

        return endereco is null ? null : EnderecoResponse.FromDomain(endereco);
    }
    
    public EnderecoResponse Create(EnderecoRequest request, Guid idUsuario)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (idUsuario == Guid.Empty)
            throw new InvalidOperationException("O UsuarioId do endereco é obrigatório");

        if (string.IsNullOrWhiteSpace(request.Cep))
            throw new InvalidOperationException("O CEP do Endereco é obrigatório");
        
        if (string.IsNullOrWhiteSpace(request.Estado))
            throw new InvalidOperationException("O estado do Endereco é obrigatório");

        if (string.IsNullOrWhiteSpace(request.Cidade))
            throw new InvalidOperationException("A cidade do Endereco é obrigatório");
        
        if (string.IsNullOrWhiteSpace(request.Bairro))
            throw new InvalidOperationException("O bairro do Endereco é obrigatório");
        
        if (string.IsNullOrWhiteSpace(request.Rua))
            throw new InvalidOperationException("A rua do Endereco é obrigatório");

        var usuarioExiste = bibliotecaElmContext.Usuarios
            .FirstOrDefault(u => u.Id == idUsuario) is not null;

        if (!usuarioExiste)
            throw new InvalidOperationException("Usuário não encontrado");
        
        if (ExistsByIdUsuario(idUsuario))
            throw new InvalidOperationException("Já existe um endereco para este usuário");

        var endereco = request.ToDomain(idUsuario);

        bibliotecaElmContext.Enderecos.Add(endereco);
        bibliotecaElmContext.SaveChanges();

        return EnderecoResponse.FromDomain(endereco);
    }

    public EnderecoResponse? Update(Guid id, EnderecoRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (id == Guid.Empty)
            throw new InvalidOperationException("O Id do endereco é obrigatório");

        if (string.IsNullOrWhiteSpace(request.Cep))
            throw new InvalidOperationException("O CEP do Endereco é obrigatório");

        if (string.IsNullOrWhiteSpace(request.Estado))
            throw new InvalidOperationException("O estado do Endereco é obrigatório");

        if (string.IsNullOrWhiteSpace(request.Cidade))
            throw new InvalidOperationException("A cidade do Endereco é obrigatório");

        if (string.IsNullOrWhiteSpace(request.Bairro))
            throw new InvalidOperationException("O bairro do Endereco é obrigatório");

        if (string.IsNullOrWhiteSpace(request.Rua))
            throw new InvalidOperationException("A rua do Endereco é obrigatório");

        var endereco = bibliotecaElmContext.Enderecos
            .FirstOrDefault(e => e.Id == id);

        if (endereco is null)
            return null;

        var enderecoEntry = bibliotecaElmContext.Entry(endereco);
        enderecoEntry.Property(e => e.Cep).CurrentValue = request.Cep;
        enderecoEntry.Property(e => e.Estado).CurrentValue = request.Estado;
        enderecoEntry.Property(e => e.Cidade).CurrentValue = request.Cidade;
        enderecoEntry.Property(e => e.Bairro).CurrentValue = request.Bairro;
        enderecoEntry.Property(e => e.Rua).CurrentValue = request.Rua;

        bibliotecaElmContext.SaveChanges();

        return EnderecoResponse.FromDomain(endereco);
    }
    
    public bool ExistsById(Guid id)
    {
        var endereco = bibliotecaElmContext.Enderecos
            .FirstOrDefault(f => f.Id == id);

        return endereco is not null;
    }
    
    public bool ExistsByIdUsuario(Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
            return false;
        
        var endereco = bibliotecaElmContext.Enderecos
            .FirstOrDefault(e => e.UsuarioId == usuarioId);

        return endereco is not null;
    }
    
    public bool Delete(Guid id)
    {
        var endereco = bibliotecaElmContext.Enderecos.FirstOrDefault(f => f.Id == id);
        if (endereco is null)
            return false;

        bibliotecaElmContext.Enderecos.Remove(endereco);
        bibliotecaElmContext.SaveChanges();

        return true;
    }
}