namespace BibliotecaELM.Domain.Entities;

public class LivroEmprestimo
{
    public int LivroIdLivro { get; set; }
    public int EmprestimoIdEmprestimo { get; set; }
    public int EmprestimoUsuarioIdUser { get; set; }

    // Navegação
    public Livro Livro { get; set; }
    public Emprestimo Emprestimo { get; set; }
}