using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Livro: BaseEntity
{
    public string NomeLivro { get; private set; }
    public decimal Preco { get; private set; }
    public DateOnly DataLancamento { get; private set; }
    
    public Autor Autor { get; private set; }
    
    protected Livro() { }
    
    // Compras e emprestimos relacionados.
    public ICollection<Compra> Compras { get; private set; } = new List<Compra>();
    public ICollection<Emprestimo> Emprestimos { get; private set; } = new List<Emprestimo>();

    public Livro(string nomeLivro, decimal preco, DateOnly dataLancamento, Autor autor)
    {
        this.NomeLivro = nomeLivro;
        this.Preco = preco;
        this.DataLancamento = dataLancamento;
        this.Autor = autor;
    }
}