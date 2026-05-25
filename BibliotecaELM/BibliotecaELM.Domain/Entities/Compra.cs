using BibliotecaELM.Domain.Common;
using BibliotecaELM.Domain.Enums;
using BibliotecaELM.Domain.Exceptions;

namespace BibliotecaELM.Domain.Entities;

public class Compra : BaseEntity
{
    public FormaCompraEnum FormaCompra { get; private set; }
    public DateTime DataCompra { get; private set; }
    
    public Guid UsuarioId { get; private set; }
    public List<Livro> Livros { get; private set; } = null!;

    protected Compra()
    {
    }

    public Compra(FormaCompraEnum formaCompra, DateTime dataCompra, Guid usuarioId, List<Livro> livros)
    {
        Update(formaCompra, dataCompra, usuarioId, livros);
    }

    public void Update(FormaCompraEnum formaCompra, DateTime dataCompra, Guid usuarioId, List<Livro> livros)
    {
        if (dataCompra > DateTime.Now)
            throw new BusinessRuleValidationException("A data da compra não pode ser no futuro.");

        if (usuarioId == Guid.Empty)
            throw new BusinessRuleValidationException("O ID do usuário é obrigatório.");

        if (livros == null || !livros.Any())
            throw new BusinessRuleValidationException("A compra deve possuir pelo menos um livro.");

        this.FormaCompra = formaCompra;
        this.DataCompra = dataCompra;
        this.UsuarioId = usuarioId;
        this.Livros = livros;
    }
}