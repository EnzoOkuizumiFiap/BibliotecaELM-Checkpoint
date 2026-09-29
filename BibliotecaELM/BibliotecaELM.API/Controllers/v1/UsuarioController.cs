using Asp.Versioning;
using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

/// <summary>
/// Controller responsável por gerenciar os Usuários da biblioteca.
/// Disponível nas versões v1.0 e v2.0 da API.
/// </summary>
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger) : ControllerBase
{
    /// <summary>
    /// Retorna todos os usuários cadastrados.
    /// </summary>
    /// <returns>Uma lista de usuários.</returns>
    /// <response code="200">Retorna a lista de usuários com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UsuarioResponse>))]
    public IActionResult GetAll()
    {
        var usuarios = usuarioService.GetAll();
        return Ok(usuarios);
    }

    /// <summary>
    /// Busca um usuário específico pelo seu identificador (GUID).
    /// </summary>
    /// <param name="id">GUID do usuário.</param>
    /// <returns>Os detalhes do usuário correspondente.</returns>
    /// <response code="200">Retorna o usuário encontrado.</response>
    /// <response code="404">Caso não exista usuário com o GUID informado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var usuario = usuarioService.GetById(id);
        if (usuario is null)
            return NotFound();

        return Ok(usuario);
    }

    /// <summary>
    /// Cadastra um novo usuário no sistema.
    /// </summary>
    /// <param name="request">Dados para cadastro do usuário.</param>
    /// <returns>O usuário cadastrado.</returns>
    /// <response code="201">Usuário cadastrado com sucesso.</response>
    /// <response code="400">Caso os dados sejam inválidos ou o e-mail já esteja em uso.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UsuarioResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] UsuarioRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        logger.LogInformation("Iniciando criação de usuário : {Email} TraceId {TraceId}", request.Email, traceId);

        var usuario = usuarioService.Create(request);

        logger.LogInformation("Finalizando criação de usuário : {Email} ({UsuarioId}) TraceId {TraceId}", usuario.Email, usuario.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
    }

    /// <summary>
    /// Atualiza os dados de um usuário existente.
    /// </summary>
    /// <param name="id">GUID do usuário a ser atualizado.</param>
    /// <param name="request">Novos dados do usuário.</param>
    /// <returns>O usuário atualizado.</returns>
    /// <response code="200">Usuário atualizado com sucesso.</response>
    /// <response code="400">Caso os dados fornecidos sejam inválidos.</response>
    /// <response code="404">Caso o usuário com o GUID fornecido não seja encontrado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] UsuarioRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        logger.LogInformation("Iniciando atualização de usuário : {UsuarioId} TraceId {TraceId}", id, traceId);

        var usuario = usuarioService.Update(id, request);
        if (usuario is null)
        {
            logger.LogWarning("Usuário não encontrado para atualização : {UsuarioId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        logger.LogInformation("Finalizando atualização de usuário : {UsuarioId} TraceId {TraceId}", id, traceId);

        return Ok(usuario);
    }

    /// <summary>
    /// Exclui um usuário da base de dados.
    /// </summary>
    /// <param name="id">GUID do usuário a ser excluído.</param>
    /// <returns>Sem conteúdo (204 NoContent).</returns>
    /// <response code="204">Usuário excluído com sucesso.</response>
    /// <response code="404">Caso o usuário com o GUID informado não seja encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var traceId = HttpContext.TraceIdentifier;

        logger.LogInformation("Iniciando exclusão de usuário : {UsuarioId} TraceId {TraceId}", id, traceId);

        if (!usuarioService.Delete(id))
        {
            logger.LogWarning("Usuário não encontrado para exclusão : {UsuarioId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        logger.LogInformation("Finalizando exclusão de usuário : {UsuarioId} TraceId {TraceId}", id, traceId);

        return NoContent();
    }
}
