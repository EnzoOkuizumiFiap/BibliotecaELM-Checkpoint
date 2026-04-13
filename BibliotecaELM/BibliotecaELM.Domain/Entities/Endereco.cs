using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;
public class Endereco : BaseEntity
{
    public string Cep { get; private set; }
    public string Estado { get; private set; }
    public string Cidade { get; private set; }
    public string Bairro { get; private set; }
    public string Rua { get; private set; }
    public Usuario Usuario { get; private set; }
    
    protected Endereco() { }
    
    public Endereco(string cep, string estado, string cidade, string bairro, string rua, Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(cep)) throw new ArgumentException("O CEP não pode ser nulo ou vazio.", nameof(cep));
        this.Cep = cep;

        if (string.IsNullOrWhiteSpace(estado)) throw new ArgumentException("O estado não pode ser nulo ou vazio.", nameof(estado));
        this.Estado = estado;

        if (string.IsNullOrWhiteSpace(cidade)) throw new ArgumentException("A cidade não pode ser nula ou vazia.", nameof(cidade));
        this.Cidade = cidade;

        if (string.IsNullOrWhiteSpace(bairro)) throw new ArgumentException("O bairro não pode ser nulo ou vazio.", nameof(bairro));
        this.Bairro = bairro;

        if (string.IsNullOrWhiteSpace(rua)) throw new ArgumentException("A rua não pode ser nula ou vazia.", nameof(rua));
        this.Rua = rua;
        
        this.Usuario = usuario ?? throw new ArgumentNullException(nameof(usuario), "O endereço deve pertencer a um usuário.");
    }
}