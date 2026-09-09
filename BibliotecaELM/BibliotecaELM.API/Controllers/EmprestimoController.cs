using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

/// <summary>
/// Controller responsável por gerenciar os Empréstimos de Livros.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class EmprestimoController : ControllerBase
{
    private readonly IEmprestimoService _emprestimoService;
    private readonly ILogger<EmprestimoController> _logger;

    public EmprestimoController(IEmprestimoService emprestimoService, ILogger<EmprestimoController> logger)
    {
        _emprestimoService = emprestimoService;
        _logger = logger;
    }

    /// <summary>
    /// Retorna todos os empréstimos registrados.
    /// </summary>
    /// <returns>Uma lista de empréstimos.</returns>
    /// <response code="200">Retorna a lista de empréstimos com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmprestimoResponse>))]
    public IActionResult GetAll()
    {
        var emprestimos = _emprestimoService.GetAll();
        return Ok(emprestimos);
    }

    /// <summary>
    /// Busca um empréstimo pelo seu GUID.
    /// </summary>
    /// <param name="id">GUID do empréstimo.</param>
    /// <returns>Os detalhes do empréstimo.</returns>
    /// <response code="200">Empréstimo encontrado.</response>
    /// <response code="404">Caso não exista empréstimo com o GUID informado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmprestimoResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var emprestimo = _emprestimoService.GetById(id);
        if (emprestimo is null)
            return NotFound();

        return Ok(emprestimo);
    }

    /// <summary>
    /// Registra um novo empréstimo de livros.
    /// </summary>
    /// <param name="request">Dados do empréstimo.</param>
    /// <returns>O empréstimo registrado.</returns>
    /// <response code="201">Empréstimo registrado com sucesso.</response>
    /// <response code="400">Caso os dados fornecidos sejam inválidos ou regras de negócio falhem (ex: data futura).</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EmprestimoResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] EmprestimoRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando criação de empréstimo : UsuarioId {UsuarioId} TraceId {TraceId}", request.UsuarioId, traceId);

        var emprestimo = _emprestimoService.Create(request);

        _logger.LogInformation("Finalizando criação de empréstimo : {EmprestimoId} TraceId {TraceId}", emprestimo.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = emprestimo.Id }, emprestimo);
    }

    /// <summary>
    /// Atualiza os dados de um empréstimo existente.
    /// </summary>
    /// <param name="id">GUID do empréstimo.</param>
    /// <param name="request">Novos dados do empréstimo.</param>
    /// <returns>O empréstimo atualizado.</returns>
    /// <response code="200">Empréstimo atualizado com sucesso.</response>
    /// <response code="400">Caso os dados de entrada falhem nas validações.</response>
    /// <response code="404">Caso o empréstimo com o GUID informado não seja encontrado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmprestimoResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] EmprestimoRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando atualização de empréstimo : {EmprestimoId} TraceId {TraceId}", id, traceId);

        var emprestimo = _emprestimoService.Update(id, request);
        if (emprestimo is null)
        {
            _logger.LogWarning("Empréstimo não encontrado para atualização : {EmprestimoId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        _logger.LogInformation("Finalizando atualização de empréstimo : {EmprestimoId} TraceId {TraceId}", id, traceId);

        return Ok(emprestimo);
    }

    /// <summary>
    /// Exclui um registro de empréstimo.
    /// </summary>
    /// <param name="id">GUID do empréstimo a remover.</param>
    /// <returns>Sem conteúdo (204 NoContent).</returns>
    /// <response code="204">Empréstimo excluído com sucesso.</response>
    /// <response code="404">Caso o empréstimo com o GUID informado não exista.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation("Iniciando exclusão de empréstimo : {EmprestimoId} TraceId {TraceId}", id, traceId);

        if (!_emprestimoService.Delete(id))
        {
            _logger.LogWarning("Empréstimo não encontrado para exclusão : {EmprestimoId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        _logger.LogInformation("Finalizando exclusão de empréstimo : {EmprestimoId} TraceId {TraceId}", id, traceId);

        return NoContent();
    }
}
