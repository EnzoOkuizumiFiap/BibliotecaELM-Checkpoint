namespace BibliotecaELM.Domain.Entities;

public class Compra
{
    public string forma_compra { get; private set; }
    public DateOnly data_compra { get; private set; }
    
    public Usuario Usuario { get; private set; }

    public Compra(string forma_compra, DateOnly data_compra, Usuario usuario)
    {
        validateFormaCompra(forma_compra);
        validateDataCompra(data_compra);
        this.Usuario = usuario;
    }

    public void validateFormaCompra(string forma)
    {
        if (forma.ToLower() == "debito" || forma.ToLower() == "credito" || forma.ToLower() == "dinheiro" ||
            forma.ToLower() == "pix")
        {
            forma_compra = forma;
        }
        else
        {
            throw new Exception("Insira um valor válido.");
        }
    }

    public void validateDataCompra(DateOnly data)
    {
        if (data.Year < 1900 && data.Year > DateTime.Now.Year)
        {
            throw new Exception("Insira um valor valido.");
        }
        else
        {
            data_compra = data;
        }
    }
}