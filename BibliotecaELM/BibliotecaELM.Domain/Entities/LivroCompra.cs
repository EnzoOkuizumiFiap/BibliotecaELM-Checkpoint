namespace BibliotecaELM.Domain.Entities;

public class LivroCompra
{
    public int LivroIdLivro { get; set; }
    public int CompraIdCompra { get; set; }
    public int CompraIdUser { get; set; }

    // Navegação
    public Livro Livro { get; set; }
    public Compra Compra { get; set; }
}