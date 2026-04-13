using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Autor : BaseEntity
{
    public string NomeAutor { get; private set; }
    public DateOnly Nascimento { get; private set; }

    public List<Livro> Livros { get; private set; }
    
    

    protected Autor()
    {
    }
    
    public Autor(string nomeAutor, DateOnly nascimento, List<Livro> livros)
    {
        this.NomeAutor = nomeAutor;
        this.Nascimento = nascimento;
        this.Livros = livros;
    }
}