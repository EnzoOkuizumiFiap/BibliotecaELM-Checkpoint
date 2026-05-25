using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
using BibliotecaELM.Infrastructure.Persistence;

namespace BibliotecaELM.Infrastructure.Repositories;

public sealed class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
{
    public EnderecoRepository(BibliotecaElmContext context) : base(context)
    {
    }

    public bool ExistsByIdUsuario(Guid usuarioId)
    {
        return _context.Enderecos.Any(e => e.UsuarioId == usuarioId);
    }
}
