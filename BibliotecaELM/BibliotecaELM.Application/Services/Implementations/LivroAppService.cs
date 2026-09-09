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
        _logger.LogInformation("Iniciando criação do livro {NomeLivro} com autor de ID {AutorId}. TraceId: {TraceId}", 
            request.NomeLivro, request.AutorId, traceId);

        if (!request.AutorId.HasValue)
        {
            _logger.LogWarning("Tentativa de criação de livro falhou: AutorId nulo. TraceId: {TraceId}", traceId);
            throw new ArgumentException("O ID do autor é obrigatório.");
        }

        var livro = new Livro(request.NomeLivro, request.Preco, request.DataLancamento, request.AutorId.Value);

        _repository.Add(livro);

        _logger.LogInformation("Livro {LivroId} criado com sucesso. TraceId: {TraceId}", 
            livro.Id, traceId);

        return new LivroResponse(
            livro.Id,
            livro.NomeLivro,
            livro.Preco,
            livro.DataLancamento,
            livro.AutorId
        );
    }
}