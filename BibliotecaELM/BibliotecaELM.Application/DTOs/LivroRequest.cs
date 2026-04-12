using System.ComponentModel.DataAnnotations;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record LivroRequest (
    [property: Required(ErrorMessage = "O nome do livro é obrigatório")]
    [property: StringLength(200, MinimumLength = 2, ErrorMessage = "O Nome do Livro deve ter entre 2 e 200 caracteres")]
    string NomeLivro,
    
    [property: Required(ErrorMessage = "O preço é obrigatório")]
    [property: Range(0.01, 9999.99, ErrorMessage = "O preço deve ser maior que 0 e menor que 10000")]
    decimal Preco,
    
    [property: Required(ErrorMessage = "A data de lançamento é obrigatória")]
    [property: Range(typeof(DateTime), "1500-01-01", "2026-04-20", ErrorMessage = "A data deve estar entre 1500 e 2026")]
    DateOnly DataLancamento,
    
    [property: Required(ErrorMessage = "O Autor é obrigatório")]
    [property: StringLength(200, MinimumLength = 2, ErrorMessage = "O Nome do Autor deve ter entre 2 e 50 caracteres")]
    Autor Autor
)
{
    public Livro ToDomain() => new Livro(NomeLivro, Preco, DataLancamento, Autor);
}