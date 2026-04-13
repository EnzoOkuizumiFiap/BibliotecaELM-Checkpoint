using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Livro: BaseEntity
{
    public string NomeLivro { get; private set; }
    public decimal Preco { get; private set; }
    public DateOnly DataLancamento { get; private set; }
    
    public Guid AutorId { get; private set; }
    public Autor? Autor { get; private set; }
    
    // Compras e emprestimos relacionados.
    public List<Compra> Compras { get; private set; } = new List<Compra>();
    public List<Emprestimo> Emprestimos { get; private set; } = new List<Emprestimo>();

    protected Livro()
    {
    }

    public Livro(string nomeLivro, decimal preco, DateOnly dataLancamento, Guid autorId)
    {
        this.NomeLivro = nomeLivro;
        this.Preco = preco;
        this.DataLancamento = dataLancamento;
        this.AutorId = autorId;
    }
}