using BibliotecaELM.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.API.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Ocorreu uma exceção não tratada: {Message}", exception.Message);

        var (statusCode, title, detail) = exception switch
        {
            BusinessRuleValidationException => (
                StatusCodes.Status400BadRequest,
                "Regra de Negócio Violada",
                exception.Message
            ),
            ResourceNotFoundException => (
                StatusCodes.Status404NotFound,
                "Recurso não encontrado",
                exception.Message
            ),
            ArgumentException => (
                StatusCodes.Status400BadRequest,
                "Requisição Inválida",
                exception.Message
            ),
            InvalidOperationException => (
                StatusCodes.Status400BadRequest,
                "Operação Inválida",
                exception.Message
            ),
            KeyNotFoundException => (
                StatusCodes.Status404NotFound,
                "Recurso não encontrado",
                exception.Message
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Erro interno do servidor",
                "Ocorreu um erro inesperado no processamento da requisição."
            )
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
