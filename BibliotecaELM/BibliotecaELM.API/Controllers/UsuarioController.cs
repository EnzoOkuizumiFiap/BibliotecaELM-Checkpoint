using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações de Usuários.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger)
    {
        _usuarioService = usuarioService;
        _logger = logger;
    }

    /// <summary>
    /// Retorna todos os usuários cadastrados.
    /// </summary>
    /// <returns>Uma lista de usuários.</returns>
    /// <response code="200">Retorna a lista de usuários com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UsuarioResponse>))]
    public IActionResult GetAll()
    {
        var usuarios = _usuarioService.GetAll();
        return Ok(usuarios);
    }

    /// <summary>
    /// Busca um usuário pelo seu GUID.
    /// </summary>
    /// <param name="id">GUID do usuário.</param>
    /// <returns>Os detalhes do usuário.</returns>
    /// <response code="200">Usuário encontrado.</response>
    /// <response code="404">Caso não exista usuário com o GUID fornecido.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var usuario = _usuarioService.GetById(id);
        if (usuario is null)
            return NotFound();

        return Ok(usuario);
    }

    /// <summary>
    /// Cadastra um novo usuário na base de dados.
    /// </summary>
    /// <param name="request">Dados do usuário a ser cadastrado.</param>
    /// <returns>Os dados do usuário cadastrado.</returns>
    /// <response code="201">Usuário cadastrado com sucesso.</response>
    /// <response code="400">Caso os dados de entrada sejam inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UsuarioResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] UsuarioRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando criação de usuário : {Email} TraceId {TraceId}", request.Email, traceId);

        var usuario = _usuarioService.Create(request);

        _logger.LogInformation("Finalizando criação de usuário : {Email} ({UsuarioId}) TraceId {TraceId}", usuario.Email, usuario.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
    }

    /// <summary>
    /// Atualiza os dados de um usuário existente.
    /// </summary>
    /// <param name="id">GUID do usuário.</param>
    /// <param name="request">Novos dados do usuário.</param>
    /// <returns>O usuário atualizado.</returns>
    /// <response code="200">Usuário atualizado com sucesso.</response>
    /// <response code="400">Caso os dados fornecidos sejam inválidos.</response>
    /// <response code="404">Caso o usuário com o GUID informado não seja encontrado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] UsuarioRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando atualização de usuário : {UsuarioId} TraceId {TraceId}", id, traceId);

        var usuario = _usuarioService.Update(id, request);
        if (usuario is null)
        {
            _logger.LogWarning("Usuário não encontrado para atualização : {UsuarioId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        _logger.LogInformation("Finalizando atualização de usuário : {UsuarioId} TraceId {TraceId}", id, traceId);

        return Ok(usuario);
    }

    /// <summary>
    /// Remove um usuário da base de dados pelo seu GUID.
    /// </summary>
    /// <param name="id">GUID do usuário a remover.</param>
    /// <returns>Sem conteúdo (204 NoContent).</returns>
    /// <response code="204">Usuário removido com sucesso.</response>
    /// <response code="404">Caso o usuário com o GUID fornecido não exista.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando exclusão de usuário : {UsuarioId} TraceId {TraceId}", id, traceId);

        if (!_usuarioService.Delete(id))
        {
            _logger.LogWarning("Usuário não encontrado para exclusão : {UsuarioId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        _logger.LogInformation("Finalizando exclusão de usuário : {UsuarioId} TraceId {TraceId}", id, traceId);

        return NoContent();
    }
}
