using BibliotecaELM.Domain.Common;
using BibliotecaELM.Domain.Exceptions;

namespace BibliotecaELM.Domain.Entities;
public class Endereco : BaseEntity
{
    public string Cep { get; private set; } = null!;
    public string Estado { get; private set; } = null!;
    public string Cidade { get; private set; } = null!;
    public string Bairro { get; private set; } = null!;
    public string Rua { get; private set; } = null!;
    public Guid UsuarioId { get; private set; }

    protected Endereco()
    {
    }
    
    public Endereco(string cep, string estado, string cidade, string bairro, string rua, Guid usuarioId)
    {
        Update(cep, estado, cidade, bairro, rua, usuarioId);
    }

    public void Update(string cep, string estado, string cidade, string bairro, string rua, Guid usuarioId)
    {
        if (string.IsNullOrWhiteSpace(cep))
            throw new BusinessRuleValidationException("O CEP é obrigatório.");
        if (string.IsNullOrWhiteSpace(estado))
            throw new BusinessRuleValidationException("O Estado é obrigatório.");
        if (string.IsNullOrWhiteSpace(cidade))
            throw new BusinessRuleValidationException("a Cidade é obrigatória.");
        if (string.IsNullOrWhiteSpace(bairro))
            throw new BusinessRuleValidationException("O Bairro é obrigatório.");
        if (string.IsNullOrWhiteSpace(rua))
            throw new BusinessRuleValidationException("A Rua é obrigatória.");
        if (usuarioId == Guid.Empty)
            throw new BusinessRuleValidationException("O ID do usuário é obrigatório.");

        this.Cep = cep;
        this.Estado = estado;
        this.Cidade = cidade;
        this.Bairro = bairro;
        this.Rua = rua;
        this.UsuarioId = usuarioId;
    }
}