using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Livro: BaseEntity
{
    public string nome_livro { get; private set; }
    public decimal preco { get; private set; }
    public DateOnly lancamento { get; private set; }
    
    public Autor autor { get; private set; }

    public Livro(string nome_livro, decimal preco, DateOnly lancamento, Autor autor)
    {
        this.nome_livro = nome_livro;
        validatePreco(preco);
        validateLancamento(lancamento);
        this.autor = autor;
    }
    
    public void validatePreco(decimal valor)
    {
        if (valor > 0 && valor < 10000)
        {
            preco = valor;
        }
        else
        {
            throw new Exception("Insira um valor valido");
        }
    }

    public void validateLancamento(DateOnly data)
    {
        if (data.Year < 2024)
        {
            lancamento = data;
        }
        else
        {
            throw new Exception("Insira uma data valida");
        }
    }
}