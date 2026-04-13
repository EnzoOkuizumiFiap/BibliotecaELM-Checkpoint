using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Livro: BaseEntity
{
    public string NomeLivro { get; private set; }
    public decimal Preco { get; private set; }
    public DateOnly DataLancamento { get; private set; }
    
    public Guid AutorId { get; private set; }
    
    // Compras e emprestimos relacionados.
    public List<Compra> Compras { get; private set; } = new List<Compra>();
    public List<Emprestimo> Emprestimos { get; private set; } = new List<Emprestimo>();

    public Livro(string nomeLivro, decimal preco, DateOnly dataLancamento, Guid autorId)
    {
        if(string.IsNullOrWhiteSpace(nomeLivro)) throw new ArgumentException("O nome do livro não pode ser vazio.", nameof(nomeLivro));
        this.NomeLivro = nomeLivro;
        
        if(preco is <= 0 or >= 10000) throw new ArgumentOutOfRangeException(nameof(preco), "O preço deve ser maior que 0 e menor que 10000.");
        this.Preco = preco;
        
        if (dataLancamento > DateOnly.FromDateTime(DateTime.Now)) throw new ArgumentException("O ano de lançamento não pode ser uma data no futuro.", nameof(dataLancamento));
        this.DataLancamento = dataLancamento;
        
        this.AutorId = autorId;
    }
}