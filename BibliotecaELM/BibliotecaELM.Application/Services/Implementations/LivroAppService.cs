using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BibliotecaELM.Application.Services.Implementations;

public class LivroAppService : ILivroAppService
{
    private readonly ILivroRepository _repository;
    private readonly ILogger<LivroAppService> _logger;

    public LivroAppService(ILivroRepository repository, ILogger<LivroAppService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<LivroResponse> CriarLivroAsync(LivroRequest request, string traceId)
    {
        _logger.LogInformation("Iniciando criação do livro {Titulo} para o Isbn {Isbn}. TraceId: {TraceId}", 
            request.Titulo, request.Isbn, traceId);

        var livro = new Livro(request.Titulo, request.Isbn, request.AnoPublicacao);

        await _repository.AddAsync(livro);

        _logger.LogInformation("Livro {LivroId} criado com sucesso. TraceId: {TraceId}", 
            livro.Id, traceId);

        return new LivroResponse
        {
            Id = livro.Id,
            Titulo = livro.Titulo,
            Isbn = livro.Isbn,
            AnoPublicacao = livro.AnoPublicacao
        };
    }
}