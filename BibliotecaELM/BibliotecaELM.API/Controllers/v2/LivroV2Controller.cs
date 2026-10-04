using Asp.Versioning;
using BibliotecaELM.API.Extensions;
using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BibliotecaELM.API.Controllers.v2;

/// <summary>
/// Controller responsável pelas operações de Livros — Versão 2.0 (ATUAL).
/// A listagem retorna um envelope paginado com totais e indicadores de navegação.
/// Segue o padrão de nomenclatura e design estabelecido na solução Recommenda (RatingV2Controller).
/// </summary>
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/livro")]
[Route("api/v{version:apiVersion}/livros")]
[Route("api/livro")]
[Route("api/livros")]
[Produces("application/json")]
public class LivroV2Controller(ILivroService livroService, ILogger<LivroV2Controller> logger) : ControllerBase
{
    /// <summary>
    /// [v2 - ATUAL] Retorna os livros de forma paginada em um envelope com totais e metadados de navegação.
    /// Sem versão explícita na requisição, este endpoint é selecionado por padrão (DefaultApiVersion = 2.0).
    /// </summary>
    /// <param name="paginationQuery">Parâmetros de paginação por query string (page &gt;= 1, pageSize entre 1 e 100).</param>
    /// <returns>Envelope paginado contendo a lista de livros da página e dados de contagem.</returns>
    /// <response code="200">Envelope com os livros paginados retornado com sucesso (items pode ser vazio caso page exceda o total).</response>
    /// <response code="400">Caso os parâmetros de paginação sejam inválidos (page &lt; 1 ou pageSize fora da faixa de 1–100).</response>
    [HttpGet]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(typeof(PagedResponse<LivroResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult GetAllPaginated([FromQuery] PaginationQuery paginationQuery)
    {
        if (paginationQuery.TryGetError(out var message))
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Parâmetros de Paginação Inválidos",
                Detail = message,
                Instance = HttpContext.Request.Path
            });
        }

        var pagedResponse = livroService.GetPaged(paginationQuery.Page, paginationQuery.PageSize);
        return Ok(pagedResponse);
    }

    /// <summary>
    /// Busca um livro específico pelo GUID.
    /// </summary>
    /// <param name="id">GUID do livro.</param>
    /// <returns>Os dados do livro encontrado.</returns>
    /// <response code="200">Livro encontrado com sucesso.</response>
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

        logger.LogInformation("Iniciando criação de livro (v2) : {NomeLivro} TraceId {TraceId}", request.NomeLivro, traceId);

        var livro = livroService.Create(request);

        logger.LogInformation("Finalizando criação de livro (v2) : {NomeLivro} ({LivroId}) TraceId {TraceId}", livro.NomeLivro, livro.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = livro.Id, version = "2.0" }, livro);
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

        logger.LogInformation("Iniciando atualização de livro (v2) : {LivroId} TraceId {TraceId}", id, traceId);

        var livro = livroService.Update(id, request);
        if (livro is null)
        {
            logger.LogWarning("Livro não encontrado para atualização (v2) : {LivroId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        logger.LogInformation("Finalizando atualização de livro (v2) : {LivroId} TraceId {TraceId}", id, traceId);

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

        logger.LogInformation("Iniciando exclusão de livro (v2) : {LivroId} TraceId {TraceId}", id, traceId);

        if (!livroService.Delete(id))
        {
            logger.LogWarning("Livro não encontrado para exclusão (v2) : {LivroId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        logger.LogInformation("Finalizando exclusão de livro (v2) : {LivroId} TraceId {TraceId}", id, traceId);

        return NoContent();
    }
}
