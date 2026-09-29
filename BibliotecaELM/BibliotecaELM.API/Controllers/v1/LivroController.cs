using Asp.Versioning;
using BibliotecaELM.API.Extensions;
using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BibliotecaELM.Controllers;

/// <summary>
/// Controller responsável pelas operações de Livros — Versão 1.0 (DEPRECADA).
/// A listagem retorna todos os livros em uma lista completa (sem paginação), preservando o contrato antigo do CP3.
/// </summary>
[ApiController]
[ApiVersion("1.0", Deprecated = true)]
[Route("api/v{version:apiVersion}/livro")]
[Route("api/v{version:apiVersion}/livros")]
[Route("api/livro")]
[Route("api/livros")]
[Produces("application/json")]
public class LivroController(ILivroService livroService, ILogger<LivroController> logger) : ControllerBase
{
    /// <summary>
    /// [v1 - DEPRECADO] Retorna todos os livros do acervo em uma lista completa (sem paginação).
    /// Favor migrar para a versão v2.0 (/api/livro?api-version=2.0) para obter o envelope paginado.
    /// </summary>
    /// <returns>Lista completa de livros.</returns>
    /// <response code="200">Retorna a lista de livros com sucesso.</response>
    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LivroResponse>))]
    public IActionResult GetAll()
    {
        var livros = livroService.GetAll();
        return Ok(livros);
    }

    /// <summary>
    /// Busca um livro específico pelo GUID.
    /// </summary>
    /// <param name="id">GUID do livro.</param>
    /// <returns>Os dados do livro encontrado.</returns>
    /// <response code="200">Livro encontrado.</response>
    /// <response code="404">Livro não encontrado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LivroResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var livro = livroService.GetById(id);
        return livro is null ? NotFound() : Ok(livro);
    }

    /// <summary>
    /// Cadastra um novo livro no acervo da biblioteca.
    /// Taxa limitada pela política Fixed Window (10 requisições por minuto por IP).
    /// </summary>
    /// <param name="request">Dados do livro a ser cadastrado.</param>
    /// <returns>O livro cadastrado.</returns>
    /// <response code="201">Livro cadastrado com sucesso.</response>
    /// <response code="400">Dados inválidos ou autor não encontrado.</response>
    /// <response code="429">Limite de requisições excedido.</response>
    [HttpPost]
    [EnableRateLimiting(RateLimitingExtensions.FixedPolicy)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LivroResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public IActionResult Create([FromBody] LivroRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        logger.LogInformation("Iniciando criação de livro (v1) : {NomeLivro} TraceId {TraceId}", request.NomeLivro, traceId);

        var livro = livroService.Create(request);

        logger.LogInformation("Finalizando criação de livro (v1) : {NomeLivro} ({LivroId}) TraceId {TraceId}", livro.NomeLivro, livro.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = livro.Id, version = "1.0" }, livro);
    }

    /// <summary>
    /// Atualiza os dados de um livro do acervo.
    /// </summary>
    /// <param name="id">GUID do livro.</param>
    /// <param name="request">Novos dados do livro.</param>
    /// <returns>O livro atualizado.</returns>
    /// <response code="200">Livro atualizado com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="404">Livro não encontrado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LivroResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] LivroRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        logger.LogInformation("Iniciando atualização de livro (v1) : {LivroId} TraceId {TraceId}", id, traceId);

        var livro = livroService.Update(id, request);
        if (livro is null)
        {
            logger.LogWarning("Livro não encontrado para atualização (v1) : {LivroId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        logger.LogInformation("Finalizando atualização de livro (v1) : {LivroId} TraceId {TraceId}", id, traceId);

        return Ok(livro);
    }

    /// <summary>
    /// Exclui um livro do acervo pelo identificador.
    /// </summary>
    /// <param name="id">GUID do livro a ser excluído.</param>
    /// <returns>Sem conteúdo (204 NoContent).</returns>
    /// <response code="204">Livro excluído com sucesso.</response>
    /// <response code="404">Livro não encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var traceId = HttpContext.TraceIdentifier;

        logger.LogInformation("Iniciando exclusão de livro (v1) : {LivroId} TraceId {TraceId}", id, traceId);

        if (!livroService.Delete(id))
        {
            logger.LogWarning("Livro não encontrado para exclusão (v1) : {LivroId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        logger.LogInformation("Finalizando exclusão de livro (v1) : {LivroId} TraceId {TraceId}", id, traceId);

        return NoContent();
    }
}
