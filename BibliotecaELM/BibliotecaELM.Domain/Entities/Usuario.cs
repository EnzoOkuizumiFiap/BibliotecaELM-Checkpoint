using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Usuario : BaseEntity
{
    public string NomeUsuario { get; private set; }
    public DateOnly Nascimento { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; private set; }
    //1..N
    public List<Emprestimo> Emprestimos { get; private set; } = new List<Emprestimo>();
    //1..N
    public List<Compra> Compras { get; private set; } = new List<Compra>();
    //1..1 opcional no usuario
    public Endereco? Endereco { get; private set; }

    protected Usuario()
    {
    }
    
    public Usuario(string nome, DateOnly nascimento, string email, string cpf, Endereco? endereco)
    {
        this.NomeUsuario = nome;
        this.Nascimento = nascimento;
        this.Email = email;
        this.Cpf = cpf;
        this.Endereco = endereco;
    }
    
    // private static int CalculateAge(DateOnly date)
    // {
    //     var today = DateOnly.FromDateTime(DateTime.Today);
    //     var age = today.Year - date.Year;
    //     if (date > today.AddYears(-age)) age--;
    //     return age;
    // }
}