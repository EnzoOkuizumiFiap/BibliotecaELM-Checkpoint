using BibliotecaELM.Domain.Common;
using BibliotecaELM.Domain.Exceptions;

namespace BibliotecaELM.Domain.Entities;

public class Usuario : BaseEntity
{
    public string NomeUsuario { get; private set; } = null!;
    public DateOnly Nascimento { get; private set; }
    public string Email { get; private set; } = null!;
    public string Cpf { get; private set; } = null!;
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
        Update(nome, nascimento, email, cpf);
        this.Endereco = endereco;
    }

    public void Update(string nome, DateOnly nascimento, string email, string cpf)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessRuleValidationException("O nome do usuário é obrigatório.");
            
        if (string.IsNullOrWhiteSpace(email))
            throw new BusinessRuleValidationException("O email do usuário é obrigatório.");
            
        if (string.IsNullOrWhiteSpace(cpf))
            throw new BusinessRuleValidationException("O CPF do usuário é obrigatório.");

        this.NomeUsuario = nome;
        this.Nascimento = nascimento;
        this.Email = email;
        this.Cpf = cpf;
    }
}