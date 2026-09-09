using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

/// <summary>
/// Controller responsável por gerenciar as transações de Compras de Livros.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class CompraController : ControllerBase
{
    private readonly ICompraService _compraService;
    private readonly ILogger<CompraController> _logger;

    public CompraController(ICompraService compraService, ILogger<CompraController> logger)
    {
        _compraService = compraService;
        _logger = logger;
    }

    /// <summary>
    /// Retorna todas as compras registradas na biblioteca.
    /// </summary>
    /// <returns>Uma lista de compras realizadas.</returns>
    /// <response code="200">Retorna a lista de compras com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompraResponse>))]
    public IActionResult GetAll()
    {
        var compras = _compraService.GetAll();
        return Ok(compras);
    }

    /// <summary>
    /// Busca uma compra específica pelo seu identificador (GUID).
    /// </summary>
    /// <param name="id">GUID da compra.</param>
    /// <returns>Os detalhes da compra correspondente.</returns>
    /// <response code="200">Retorna a compra encontrada.</response>
    /// <response code="404">Caso não exista compra com o GUID informado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompraResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var compra = _compraService.GetById(id);
        if (compra is null)
            return NotFound();

        return Ok(compra);
    }

    /// <summary>
    /// Registra uma nova compra de livros na base de dados.
    /// </summary>
    /// <param name="request">Dados para registro da compra.</param>
    /// <returns>A compra registrada.</returns>
    /// <response code="201">Compra registrada com sucesso.</response>
    /// <response code="400">Caso os dados de entrada sejam inválidos, data futura ou se o usuário não possuir endereço cadastrado.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CompraResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CompraRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando criação de compra : UsuarioId {UsuarioId} TraceId {TraceId}", request.UsuarioId, traceId);

        var compra = _compraService.Create(request);

        _logger.LogInformation("Finalizando criação de compra : {CompraId} TraceId {TraceId}", compra.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = compra.Id }, compra);
    }

    /// <summary>
    /// Atualiza os dados de uma compra existente.
    /// </summary>
    /// <param name="id">GUID da compra.</param>
    /// <param name="request">Novos dados da compra.</param>
    /// <returns>A compra atualizada.</returns>
    /// <response code="200">Compra atualizada com sucesso.</response>
    /// <response code="400">Caso as regras de validação falhem.</response>
    /// <response code="404">Caso a compra com o GUID fornecido não seja encontrada.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompraResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] CompraRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando atualização de compra : {CompraId} TraceId {TraceId}", id, traceId);

        var compra = _compraService.Update(id, request);
        if (compra is null)
        {
            _logger.LogWarning("Compra não encontrada para atualização : {CompraId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        _logger.LogInformation("Finalizando atualização de compra : {CompraId} TraceId {TraceId}", id, traceId);

        return Ok(compra);
    }

    /// <summary>
    /// Exclui uma compra da base de dados.
    /// </summary>
    /// <param name="id">GUID da compra a ser excluída.</param>
    /// <returns>Sem conteúdo (204 NoContent).</returns>
    /// <response code="204">Compra excluída com sucesso.</response>
    /// <response code="404">Caso a compra com o GUID informado não seja encontrada.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando exclusão de compra : {CompraId} TraceId {TraceId}", id, traceId);

        if (!_compraService.Delete(id))
        {
            _logger.LogWarning("Compra não encontrada para exclusão : {CompraId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        _logger.LogInformation("Finalizando exclusão de compra : {CompraId} TraceId {TraceId}", id, traceId);

        return NoContent();
    }
}
