using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Compra : BaseEntity
{
    public string FormaCompra { get; private set; }
    public DateOnly DataCompra { get; private set; }
    
    // Propriedades de navegação
    public Usuario Usuario { get; private set; }
    public List<Livro> Livros { get; private set; }

    public Compra(string formaCompra, DateOnly dataCompra, Usuario usuario, List<Livro> livros)
    {
        if (string.IsNullOrWhiteSpace(formaCompra) || 
            !(formaCompra.Equals("débito", StringComparison.CurrentCultureIgnoreCase) || 
              formaCompra.Equals("crédito", StringComparison.CurrentCultureIgnoreCase) || 
              formaCompra.Equals("dinheiro", StringComparison.CurrentCultureIgnoreCase) || 
              formaCompra.Equals("pix", StringComparison.CurrentCultureIgnoreCase))) 
            throw new ArgumentException("Forma de pagamento inválida. Use: débito, crédito, dinheiro ou pix.", nameof(formaCompra)); 
        this.FormaCompra = formaCompra;
        
        if (dataCompra.Year < 1900 || dataCompra.Year > DateTime.Now.Year) throw new ArgumentOutOfRangeException(nameof(dataCompra), "A data de compra não pode ser anterior a 1900 ou no futuro.");
        this.DataCompra = dataCompra;
        
        this.Usuario = usuario ?? throw new ArgumentNullException(nameof(usuario), "O usuário não pode ser nulo.");
        this.Livros = livros ?? throw new ArgumentNullException(nameof(livros), "O livro não pode ser nulo.");
    }
}