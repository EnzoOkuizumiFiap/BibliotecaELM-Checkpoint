namespace BibliotecaELM.Domain.Common;

public class BaseEntity
{
    public Guid id { get; private set; } = Guid.NewGuid();
}