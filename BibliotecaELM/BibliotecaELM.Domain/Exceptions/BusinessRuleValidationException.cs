using System;

namespace BibliotecaELM.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando uma regra de negócio ou validação falha.
/// </summary>
public class BusinessRuleValidationException : Exception
{
    public BusinessRuleValidationException(string message) : base(message)
    {
    }

    public BusinessRuleValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
