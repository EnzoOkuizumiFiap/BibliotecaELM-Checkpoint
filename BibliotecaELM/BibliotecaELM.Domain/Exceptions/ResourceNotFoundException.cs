using System;

namespace BibliotecaELM.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando um recurso solicitado não é encontrado.
/// </summary>
public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string message) : base(message)
    {
    }

    public ResourceNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
