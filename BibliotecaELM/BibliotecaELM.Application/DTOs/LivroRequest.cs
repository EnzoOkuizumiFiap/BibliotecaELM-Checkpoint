using System.ComponentModel.DataAnnotations;
using BibliotecaELM.Application.Validation;
using BibliotecaELM.Domain.Entities;

namespace BibliotecaELM.Application.DTOs;

public record LivroRequest (
    [param: Required(ErrorMessage = "O nome do livro é obrigatório")]
    [param: StringLength(200, MinimumLength = 2, ErrorMessage = "O Nome do Livro deve ter entre 2 e 200 caracteres")]
    string NomeLivro,
    
    [param: Required(ErrorMessage = "O preço é obrigatório")]
    [param: Range(0.01, 9999.99, ErrorMessage = "O preço deve ser maior que 0 e menor que 10000")]
    decimal Preco,
    
    [param: Required(ErrorMessage = "A data de lançamento é obrigatória")]
    [param: DateOnlyRange("1500-01-01", "2026-04-20", ErrorMessage = "A data deve estar entre 1500 e 2026")]
    DateOnly DataLancamento,
    
    [param: Required(ErrorMessage = "O AutorId é obrigatório")]
    Guid? AutorId
)
{
    public Livro ToDomain() => new Livro(NomeLivro, Preco, DataLancamento, AutorId!.Value);
}