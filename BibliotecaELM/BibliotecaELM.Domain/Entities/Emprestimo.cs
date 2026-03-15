using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Emprestimo : BaseEntity
{
    public DateTime DataEmprestimo { get; private set; }
    public DateTime DataDevolucao { get; private set; }
    
    // Propriedades de navegação
    public Usuario Usuario { get; private set; }
    public List<Livro> Livros { get; private set; }
    
    public Emprestimo(DateTime dataEmprestimo, DateTime dataDevolucao, Usuario usuario, List<Livro> livros)
    {
        if (dataDevolucao < dataEmprestimo) throw new ArgumentException("A data de devolução não pode ser anterior à data de empréstimo.");
        this.DataEmprestimo = dataEmprestimo;
        this.DataDevolucao = dataDevolucao;
        
        this.Usuario = usuario ?? throw new ArgumentNullException(nameof(usuario), "O usuário não pode ser nulo.");
        
        if (livros == null || livros.Count == 0) throw new ArgumentException("A compra deve possuir ao menos um livro.", nameof(livros));
        this.Livros = livros;
    }
}