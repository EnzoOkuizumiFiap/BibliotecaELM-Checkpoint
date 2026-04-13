using BibliotecaELM.Domain.Common;
using BibliotecaELM.Domain.Enums;

namespace BibliotecaELM.Domain.Entities;

public class Compra : BaseEntity
{
    public FormaCompraEnum FormaCompra { get; private set; }
    public DateTime DataCompra { get; private set; }
    
    public Usuario Usuario { get; private set; }
    public ICollection<Livro> Livros { get; private set; }
    
    protected Compra() { }

    public Compra(FormaCompraEnum formaCompra, DateTime dataCompra, Usuario usuario, ICollection<Livro> livros)
    {
        this.FormaCompra = formaCompra;
        
        if (dataCompra.Date < new DateTime(1900, 1, 1) || dataCompra.Date > DateTime.Today) throw new ArgumentOutOfRangeException(nameof(dataCompra), "A data de compra deve estar entre 01/01/1900 e hoje.");
        this.DataCompra = dataCompra;
        
        this.Usuario = usuario ?? throw new ArgumentNullException(nameof(usuario), "O usuário não pode ser nulo.");
        
        if (livros == null || livros.Count == 0) throw new ArgumentException("A compra deve possuir ao menos um livro.", nameof(livros));
        this.Livros = livros;
    }
}