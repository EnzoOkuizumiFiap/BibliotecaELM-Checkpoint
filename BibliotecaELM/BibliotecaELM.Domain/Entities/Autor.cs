using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Autor : BaseEntity
{
    public string NomeAutor { get; private set; } = null!;
    public DateOnly Nascimento { get; private set; }

    public List<Livro> Livros { get; private set; } = null!;
    
    

    protected Autor()
    {
    }
    
    public Autor(string nomeAutor, DateOnly nascimento, List<Livro> livros)
    {
        this.NomeAutor = nomeAutor;
        this.Nascimento = nascimento;
        this.Livros = livros;
    }

    public void Update(string nomeAutor, DateOnly nascimento)
    {
        this.NomeAutor = nomeAutor;
        this.Nascimento = nascimento;
    }
}