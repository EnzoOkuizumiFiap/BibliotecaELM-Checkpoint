using System.Net.Mime;
using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Autor : BaseEntity
{
    public string nome_autor { get; private set; }
    public DateOnly nascimento { get; private set; }

    public Autor(string nome, DateOnly nascimento)
    {
        this.nome_autor = nome;
        ValidateLancamento(nascimento);
    }

    public void ValidateLancamento(DateOnly data)
    {
        if (data.Year < 2015)
        {
            nascimento = data;
        }
        else
        {
            throw new Exception("insira um valor válido.");
        }
    }
}