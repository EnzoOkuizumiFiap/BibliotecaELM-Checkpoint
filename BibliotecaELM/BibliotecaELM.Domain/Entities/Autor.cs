using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Autor : BaseEntity
{
    public string NomeAutor { get; private set; }
    public string Descricao { get; private set; }
    public DateOnly Nascimento { get; private set; }

    // Propriedades de navegação
    public List<Livro> Livros { get; private set; }
    
    public Autor(string nomeAutor, string descricao, DateOnly nascimento, List<Livro> livros)
    {
        if (string.IsNullOrWhiteSpace(nomeAutor)) throw new ArgumentException("O nome do autor não pode ser vazio ou nulo.", nameof(nomeAutor));
        this.NomeAutor = nomeAutor;
        
        if (string.IsNullOrWhiteSpace(descricao)) throw new ArgumentException("A descrição do autor não pode ser vazia.", nameof(descricao));
        this.Descricao = descricao;
        
        DateOnly dataAtual = DateOnly.FromDateTime(DateTime.Now);
        if (nascimento > dataAtual) throw new ArgumentOutOfRangeException(nameof(nascimento), "A data de nascimento não pode estar no futuro.");
        if (nascimento.Year < 1000) throw new ArgumentOutOfRangeException(nameof(nascimento), "O ano de nascimento é inválido (muito antigo).");
        this.Nascimento = nascimento;
        
        this.Livros = livros ?? throw new ArgumentNullException(nameof(livros), "O livro não pode ser nulo.");
    }
}