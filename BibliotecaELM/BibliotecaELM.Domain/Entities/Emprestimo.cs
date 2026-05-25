using BibliotecaELM.Domain.Common;
using BibliotecaELM.Domain.Exceptions;

namespace BibliotecaELM.Domain.Entities;

public class Emprestimo : BaseEntity
{
    public DateTime DataEmprestimo { get; private set; }
    public DateTime DataDevolucao { get; private set; }
    
    public Guid UsuarioId { get; private set; }
    public List<Livro> Livros { get; private set; } = null!;

    protected Emprestimo()
    {
    }
    
    public Emprestimo(DateTime dataEmprestimo, DateTime dataDevolucao, Guid usuarioId, List<Livro> livros)
    {
        Update(dataEmprestimo, dataDevolucao, usuarioId, livros);
    }

    public void Update(DateTime dataEmprestimo, DateTime dataDevolucao, Guid usuarioId, List<Livro> livros)
    {
        if (dataEmprestimo > DateTime.Now)
            throw new BusinessRuleValidationException("A data do empréstimo não pode ser no futuro.");

        if (dataDevolucao < dataEmprestimo)
            throw new BusinessRuleValidationException("A data de devolução não pode ser anterior à data do empréstimo.");

        if (usuarioId == Guid.Empty)
            throw new BusinessRuleValidationException("O ID do usuário é obrigatório.");

        if (livros == null || !livros.Any())
            throw new BusinessRuleValidationException("O empréstimo deve possuir pelo menos um livro.");

        this.DataEmprestimo = dataEmprestimo;
        this.DataDevolucao = dataDevolucao;
        this.UsuarioId = usuarioId;
        this.Livros = livros;
    }
}