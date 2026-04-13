using BibliotecaELM.Domain.Common;

namespace BibliotecaELM.Domain.Entities;
public class Endereco : BaseEntity
{
    public string Cep { get; private set; }
    public string Estado { get; private set; }
    public string Cidade { get; private set; }
    public string Bairro { get; private set; }
    public string Rua { get; private set; }
    public Guid UsuarioId { get; private set; }

    protected Endereco()
    {
    }
    
    public Endereco(string cep, string estado, string cidade, string bairro, string rua, Guid usuarioId)
    {
        this.Cep = cep;
        this.Estado = estado;
        this.Cidade = cidade;
        this.Bairro = bairro;
        this.Rua = rua;
        this.UsuarioId = usuarioId;
    }
}