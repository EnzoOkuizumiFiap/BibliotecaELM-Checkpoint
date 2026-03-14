using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;

public class Usuario : BaseEntity
{
    public string nome_user { get; private set; }
    public DateOnly data_user { get; private set; }
    public string email { get; private set; }
    public int cpf { get; private set; }
    
    public List<Emprestimo> emprestimos { get; private set; }
    public Endereco endereco { get; private set; }
    public List<Compra> compras { get; private set; }
    public List<Livro> livros { get; private set; }
    
    public Usuario(string nome_user, DateOnly data_user, string email, int cpf)
    {
        UpdateName(nome_user);
        UpdateEmail(email);
        SetBirthDate(data_user);
        ValidateCpf(cpf);
    }
    
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new Exception("Nome não pode ser vazio.");
        
        nome_user = newName;
    }

    public void UpdateEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
            throw new Exception("E-mail inválido.");
            
        email = newEmail;
    }
    
    public void SetBirthDate(DateOnly newDate)
    {
        var age = CalculateAge(newDate);
        
        if (age < 10 && age < 100)
            throw new Exception("Insira um valor válido.");

        data_user = newDate;
    }
    
    private static int CalculateAge(DateOnly date)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - date.Year;
        if (date > today.AddYears(-age)) age--;
        return age;
    }
    
    private void ValidateCpf(int cpf)
    {
        if (cpf.ToString().Length == 11)
        {
            this.cpf = cpf;
        }
        else
        {
            throw new Exception("Insira um valor válido.");
        }
    }
}