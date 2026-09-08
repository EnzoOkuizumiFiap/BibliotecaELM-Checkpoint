namespace BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Application.DTOs;

public interface ILivroAppService
{
    Task<LivroResponse> CriarLivroAsync(LivroRequest request, string traceId);
}