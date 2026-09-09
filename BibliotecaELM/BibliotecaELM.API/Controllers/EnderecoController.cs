using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

/// <summary>
/// Controller responsável por gerenciar os Endereços dos Usuários.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class EnderecoController : ControllerBase
{
    private readonly IEnderecoService _enderecoService;
    private readonly ILogger<EnderecoController> _logger;

    public EnderecoController(IEnderecoService enderecoService, ILogger<EnderecoController> logger)
    {
        _enderecoService = enderecoService;
        _logger = logger;
    }

    /// <summary>
    /// Retorna todos os endereços cadastrados.
    /// </summary>
    /// <returns>Uma lista de endereços.</returns>
    /// <response code="200">Retorna a lista de endereços com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EnderecoResponse>))]
    public IActionResult GetAll()
    {
        var enderecos = _enderecoService.GetAll();
        return Ok(enderecos);
    }

    /// <summary>
    /// Busca um endereço específico pelo GUID.
    /// </summary>
    /// <param name="id">GUID do endereço.</param>
    /// <returns>Os detalhes do endereço.</returns>
    /// <response code="200">Endereço encontrado.</response>
    /// <response code="404">Caso não exista endereço com o GUID fornecido.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EnderecoResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var endereco = _enderecoService.GetById(id);
        if (endereco is null)
            return NotFound();

        return Ok(endereco);
    }

    /// <summary>
    /// Cadastra um novo endereço associado a um usuário.
    /// </summary>
    /// <param name="usuarioId">GUID do usuário dono do endereço.</param>
    /// <param name="request">Dados do endereço a ser cadastrado.</param>
    /// <returns>O endereço cadastrado.</returns>
    /// <response code="201">Endereço cadastrado com sucesso.</response>
    /// <response code="400">Caso os dados de entrada sejam inválidos ou o usuário já possua endereço cadastrado.</response>
    [HttpPost("{usuarioId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EnderecoResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create(Guid usuarioId, [FromBody] EnderecoRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando criação de endereço : UsuarioId {UsuarioId} TraceId {TraceId}", usuarioId, traceId);

        var endereco = _enderecoService.Create(request, usuarioId);

        _logger.LogInformation("Finalizando criação de endereço : {EnderecoId} TraceId {TraceId}", endereco.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = endereco.Id }, endereco);
    }

    /// <summary>
    /// Atualiza os dados de um endereço existente.
    /// </summary>
    /// <param name="id">GUID do endereço.</param>
    /// <param name="request">Novos dados do endereço.</param>
    /// <returns>O endereço atualizado.</returns>
    /// <response code="200">Endereço atualizado com sucesso.</response>
    /// <response code="400">Caso os dados fornecidos sejam inválidos.</response>
    /// <response code="404">Caso o endereço com o GUID informado não seja encontrado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EnderecoResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] EnderecoRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando atualização de endereço : {EnderecoId} TraceId {TraceId}", id, traceId);

        var endereco = _enderecoService.Update(id, request);
        if (endereco is null)
        {
            _logger.LogWarning("Endereço não encontrado para atualização : {EnderecoId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        _logger.LogInformation("Finalizando atualização de endereço : {EnderecoId} TraceId {TraceId}", id, traceId);

        return Ok(endereco);
    }

    /// <summary>
    /// Remove um endereço cadastrado.
    /// </summary>
    /// <param name="id">GUID do endereço a ser removido.</param>
    /// <returns>Sem conteúdo (204 NoContent).</returns>
    /// <response code="204">Endereço excluído com sucesso.</response>
    /// <response code="404">Caso o endereço com o GUID informado não seja encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando exclusão de endereço : {EnderecoId} TraceId {TraceId}", id, traceId);

        if (!_enderecoService.Delete(id))
        {
            _logger.LogWarning("Endereço não encontrado para exclusão : {EnderecoId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        _logger.LogInformation("Finalizando exclusão de endereço : {EnderecoId} TraceId {TraceId}", id, traceId);

        return NoContent();
    }
}
