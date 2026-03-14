namespace BibliotecaELM.Domain.Entities;

public class Emprestimo
{
    private long id_emprestimo;
    private DateTime data_emprestimo;
    private DateTime data_devolucao;
    private long id_user;
    private long id_livro;
}