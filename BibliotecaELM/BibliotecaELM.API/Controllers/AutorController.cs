using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações de Autores.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class AutorController : ControllerBase
{
    private readonly IAutorService _autorService;
    private readonly ILogger<AutorController> _logger;

    public AutorController(IAutorService autorService, ILogger<AutorController> logger)
    {
        _autorService = autorService;
        _logger = logger;
    }

    /// <summary>
    /// Retorna todos os autores cadastrados.
    /// </summary>
    /// <returns>Uma lista de autores.</returns>
    /// <response code="200">Retorna a lista de autores cadastrados com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AutorResponse>))]
    public IActionResult GetAll()
    {
        var autores = _autorService.GetAll();
        return Ok(autores);
    }

    /// <summary>
    /// Busca um autor específico pelo identificador único (GUID).
    /// </summary>
    /// <param name="id">GUID do autor.</param>
    /// <returns>Os detalhes do autor correspondente.</returns>
    /// <response code="200">Retorna o autor encontrado.</response>
    /// <response code="404">Caso não exista autor com o GUID fornecido.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AutorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var autor = _autorService.GetById(id);
        if (autor is null)
            return NotFound();

        return Ok(autor);
    }

    /// <summary>
    /// Cadastra um novo autor na base de dados.
    /// </summary>
    /// <param name="request">Dados para a criação do autor.</param>
    /// <returns>Os dados do autor cadastrado.</returns>
    /// <response code="201">Autor cadastrado com sucesso.</response>
    /// <response code="400">Caso os dados fornecidos sejam inválidos ou o nome do autor já esteja em uso.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AutorResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] AutorRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando criação de autor : {NomeAutor} TraceId {TraceId}", request.NomeAutor, traceId);

        var autor = _autorService.Create(request);

        _logger.LogInformation("Finalizando criação de autor : {NomeAutor} ({AutorId}) TraceId {TraceId}", autor.NomeAutor, autor.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = autor.Id }, autor);
    }

    /// <summary>
    /// Atualiza os dados de um autor existente.
    /// </summary>
    /// <param name="id">GUID do autor a ser atualizado.</param>
    /// <param name="request">Novos dados do autor.</param>
    /// <returns>O autor atualizado.</returns>
    /// <response code="200">Autor atualizado com sucesso.</response>
    /// <response code="400">Caso os dados fornecidos sejam inválidos ou o nome já esteja em uso por outro autor.</response>
    /// <response code="404">Caso o autor com o GUID informado não seja encontrado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AutorResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] AutorRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando atualização de autor : {AutorId} TraceId {TraceId}", id, traceId);

        var autor = _autorService.Update(id, request);
        if (autor is null)
        {
            _logger.LogWarning("Autor não encontrado para atualização : {AutorId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        _logger.LogInformation("Finalizando atualização de autor : {AutorId} TraceId {TraceId}", id, traceId);

        return Ok(autor);
    }

    /// <summary>
    /// Remove um autor da base de dados pelo seu GUID.
    /// </summary>
    /// <param name="id">GUID do autor a ser removido.</param>
    /// <returns>Sem conteúdo (204 NoContent).</returns>
    /// <response code="204">Autor deletado com sucesso.</response>
    /// <response code="404">Caso o autor com o GUID fornecido não exista.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando exclusão de autor : {AutorId} TraceId {TraceId}", id, traceId);

        if (!_autorService.Delete(id))
        {
            _logger.LogWarning("Autor não encontrado para exclusão : {AutorId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        _logger.LogInformation("Finalizando exclusão de autor : {AutorId} TraceId {TraceId}", id, traceId);

        return NoContent();
    }
    
    /// <summary>
    /// Verifica a existência de um autor através de seu nome exato (case-insensitive).
    /// </summary>
    /// <param name="nomeAutor">Nome do autor a verificar.</param>
    /// <returns>Retorna true se o autor existir.</returns>
    /// <response code="200">Autor encontrado.</response>
    /// <response code="404">Caso não exista autor com o nome informado.</response>
    [HttpGet("{nomeAutor}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ExistsByNomeAutor(string nomeAutor)
    {
        var autorNome = _autorService.ExistsByNomeAutor(nomeAutor);
        if (!autorNome)
            return NotFound();
        
        return Ok(autorNome);
    }
}
