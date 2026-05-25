using BibliotecaELM.Domain.Common;
using BibliotecaELM.Domain.Exceptions;

namespace BibliotecaELM.Domain.Entities;

public class Livro: BaseEntity
{
    public string NomeLivro { get; private set; } = null!;
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
        Update(nomeLivro, preco, dataLancamento, autorId);
    }

    public void Update(string nomeLivro, decimal preco, DateOnly dataLancamento, Guid autorId)
    {
        if (string.IsNullOrWhiteSpace(nomeLivro))
            throw new BusinessRuleValidationException("O nome do livro é obrigatório.");
            
        if (preco < 0)
            throw new BusinessRuleValidationException("O preço do livro não pode ser negativo.");

        if (autorId == Guid.Empty)
            throw new BusinessRuleValidationException("O autor do livro é obrigatório.");

        this.NomeLivro = nomeLivro;
        this.Preco = preco;
        this.DataLancamento = dataLancamento;
        this.AutorId = autorId;
    }
}