using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Emprestimo : BaseEntity
{
    public DateTime DataEmprestimo { get; private set; }
    public DateTime DataDevolucao { get; private set; }
    
    public Guid UsuarioId { get; private set; }
    public List<Livro> Livros { get; private set; }

    protected Emprestimo()
    {
    }
    
    public Emprestimo(DateTime dataEmprestimo, DateTime dataDevolucao, Guid usuarioId, List<Livro> livros)
    {
        this.DataEmprestimo = dataEmprestimo;
        this.DataDevolucao = dataDevolucao;
        this.UsuarioId = usuarioId;
        this.Livros = livros;
    }
}