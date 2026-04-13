using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Usuario : BaseEntity
{
    public string NomeUsuario { get; private set; }
    public DateOnly Nascimento { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; private set; }
    //1..N
    public List<Emprestimo> Emprestimos { get; private set; } = [];
    //1..N
    public ICollection<Compra> Compras { get; private set; } = [];
    //1..1
    public Endereco? Endereco { get; private set; }
    
    protected Usuario() { }
    
    public Usuario(string nome, DateOnly nascimento, string email, string cpf, Endereco endereco)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("O nome não pode ser vazio ou nulo.", nameof(nome));
        this.NomeUsuario = nome;
        
        var age = CalculateAge(nascimento);
        if (age is < 10 or > 100) throw new ArgumentOutOfRangeException(nameof(nascimento), "O usuário deve ter entre 10 e 100 anos.");
        this.Nascimento = nascimento;
        
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) throw new ArgumentException("E-mail inválido. Deve conter '@'.", nameof(email));
        this.Email = email;
        
        if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11) throw new ArgumentException("CPF inválido. Deve conter exatamente 11 caracteres.", nameof(cpf));
        this.Cpf = cpf;
        
        this.Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco), "O endereço não pode ser nulo.");
    }
    
    private static int CalculateAge(DateOnly date)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - date.Year;
        if (date > today.AddYears(-age)) age--;
        return age;
    }
}