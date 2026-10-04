using Asp.Versioning;
using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.API.Controllers.v1;

/// <summary>
/// Controller responsável por gerenciar os Endereços dos usuários da biblioteca.
/// Disponível nas versões v1.0 e v2.0 da API.
/// </summary>
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class EnderecoController(IEnderecoService enderecoService, ILogger<EnderecoController> logger) : ControllerBase
{
    /// <summary>
    /// Retorna todos os endereços cadastrados.
    /// </summary>
    /// <returns>Uma lista de endereços.</returns>
    /// <response code="200">Retorna a lista de endereços com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EnderecoResponse>))]
    public IActionResult GetAll()
    {
        var enderecos = enderecoService.GetAll();
        return Ok(enderecos);
    }

    /// <summary>
    /// Busca um endereço específico pelo seu identificador (GUID).
    /// </summary>
    /// <param name="id">GUID do endereço.</param>
    /// <returns>Os detalhes do endereço correspondente.</returns>
    /// <response code="200">Retorna o endereço encontrado.</response>
    /// <response code="404">Caso não exista endereço com o GUID informado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EnderecoResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var endereco = enderecoService.GetById(id);
        if (endereco is null)
            return NotFound();

        return Ok(endereco);
    }

    /// <summary>
    /// Cadastra um novo endereço associado a um usuário.
    /// </summary>
    /// <param name="usuarioId">Identificador do usuário proprietário do endereço.</param>
    /// <param name="request">Dados para cadastro do endereço.</param>
    /// <returns>O endereço cadastrado.</returns>
    /// <response code="201">Endereço cadastrado com sucesso.</response>
    /// <response code="400">Caso os dados sejam inválidos ou o usuário não exista.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EnderecoResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create(Guid usuarioId, [FromBody] EnderecoRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        logger.LogInformation("Iniciando criação de endereço : UsuarioId {UsuarioId} TraceId {TraceId}", usuarioId, traceId);

        var endereco = enderecoService.Create(request, usuarioId);

        logger.LogInformation("Finalizando criação de endereço : {EnderecoId} TraceId {TraceId}", endereco.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = endereco.Id }, endereco);
    }

    /// <summary>
    /// Atualiza os dados de um endereço existente.
    /// </summary>
    /// <param name="id">GUID do endereço a ser atualizado.</param>
    /// <param name="request">Novos dados do endereço.</param>
    /// <returns>O endereço atualizado.</returns>
    /// <response code="200">Endereço atualizado com sucesso.</response>
    /// <response code="400">Caso os dados fornecidos sejam inválidos.</response>
    /// <response code="404">Caso o endereço com o GUID fornecido não seja encontrado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EnderecoResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] EnderecoRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        logger.LogInformation("Iniciando atualização de endereço : {EnderecoId} TraceId {TraceId}", id, traceId);

        var endereco = enderecoService.Update(id, request);
        if (endereco is null)
        {
            logger.LogWarning("Endereço não encontrado para atualização : {EnderecoId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        logger.LogInformation("Finalizando atualização de endereço : {EnderecoId} TraceId {TraceId}", id, traceId);

        return Ok(endereco);
    }

    /// <summary>
    /// Exclui um endereço da base de dados.
    /// </summary>
    /// <param name="id">GUID do endereço a ser excluído.</param>
    /// <returns>Sem conteúdo (204 NoContent).</returns>
    /// <response code="204">Endereço excluído com sucesso.</response>
    /// <response code="404">Caso o endereço com o GUID informado não seja encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var traceId = HttpContext.TraceIdentifier;

        logger.LogInformation("Iniciando exclusão de endereço : {EnderecoId} TraceId {TraceId}", id, traceId);

        if (!enderecoService.Delete(id))
        {
            logger.LogWarning("Endereço não encontrado para exclusão : {EnderecoId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        logger.LogInformation("Finalizando exclusão de endereço : {EnderecoId} TraceId {TraceId}", id, traceId);

        return NoContent();
    }
}
