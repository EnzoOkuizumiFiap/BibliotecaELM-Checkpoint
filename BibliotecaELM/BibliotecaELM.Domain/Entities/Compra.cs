using BibliotecaELM.Domain.Common;
using BibliotecaELM.Domain.Enums;

namespace BibliotecaELM.Domain.Entities;

public class Compra : BaseEntity
{
    public FormaCompraEnum FormaCompra { get; private set; }
    public DateTime DataCompra { get; private set; }
    
    public Guid UsuarioId { get; private set; }
    public List<Livro> Livros { get; private set; }

    protected Compra()
    {
    }

    public Compra(FormaCompraEnum formaCompra, DateTime dataCompra, Guid usuarioId, List<Livro> livros)
    {
        this.FormaCompra = formaCompra;
        this.DataCompra = dataCompra;
        this.UsuarioId = usuarioId;
        this.Livros = livros;
    }
}