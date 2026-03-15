using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Livro: BaseEntity
{
    public string NomeLivro { get; private set; }
    public double Preco { get; private set; }
    public DateOnly DataLancamento { get; private set; }
    
    // Propriedades de navegação
    public Autor Autor { get; private set; }
    
    // Compras e emprestimos relacionados.
    public ICollection<Compra> Compras { get; set; } = new List<Compra>();
    public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();

    public Livro(string nomeLivro, double preco, DateOnly dataLancamento, Autor autor)
    {
        if(string.IsNullOrWhiteSpace(nomeLivro)) throw new ArgumentException("O nome do livro não pode ser vazio.", nameof(nomeLivro));
        this.NomeLivro = nomeLivro;
        
        if(preco is <= 0 or >= 10000) throw new ArgumentOutOfRangeException(nameof(preco), "O preço deve ser maior que 0 e menor que 10000.");
        this.Preco = preco;
        
        if (dataLancamento.Year > DateTime.Now.Year) throw new ArgumentException("O ano de lançamento não pode ser uma data no futuro.", nameof(dataLancamento));
        this.DataLancamento = dataLancamento;
        
        this.Autor = autor ?? throw new ArgumentNullException(nameof(autor), "O autor não pode ser nulo.");
    }
}