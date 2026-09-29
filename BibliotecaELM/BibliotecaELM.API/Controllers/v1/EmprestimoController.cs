using Asp.Versioning;
using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações de Empréstimos de Livros.
/// Disponível nas versões v1.0 e v2.0 da API.
/// </summary>
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class EmprestimoController(IEmprestimoService emprestimoService, ILogger<EmprestimoController> logger) : ControllerBase
{
    /// <summary>
    /// Retorna todos os empréstimos registrados na biblioteca.
    /// </summary>
    /// <returns>Uma lista de empréstimos realizados.</returns>
    /// <response code="200">Retorna a lista de empréstimos com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmprestimoResponse>))]
    public IActionResult GetAll()
    {
        var emprestimos = emprestimoService.GetAll();
        return Ok(emprestimos);
    }

    /// <summary>
    /// Busca um empréstimo específico pelo seu identificador (GUID).
    /// </summary>
    /// <param name="id">GUID do empréstimo.</param>
    /// <returns>Os detalhes do empréstimo correspondente.</returns>
    /// <response code="200">Retorna o empréstimo encontrado.</response>
    /// <response code="404">Caso não exista empréstimo com o GUID informado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmprestimoResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var emprestimo = emprestimoService.GetById(id);
        if (emprestimo is null)
            return NotFound();

        return Ok(emprestimo);
    }

    /// <summary>
    /// Registra um novo empréstimo de livros na base de dados.
    /// </summary>
    /// <param name="request">Dados para registro do empréstimo.</param>
    /// <returns>O empréstimo registrado.</returns>
    /// <response code="201">Empréstimo registrado com sucesso.</response>
    /// <response code="400">Caso os dados fornecidos violem as regras de negócio.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EmprestimoResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] EmprestimoRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        logger.LogInformation("Iniciando criação de empréstimo : UsuarioId {UsuarioId} TraceId {TraceId}", request.UsuarioId, traceId);

        var emprestimo = emprestimoService.Create(request);

        logger.LogInformation("Finalizando criação de empréstimo : {EmprestimoId} TraceId {TraceId}", emprestimo.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = emprestimo.Id }, emprestimo);
    }

    /// <summary>
    /// Atualiza os dados de um empréstimo existente.
    /// </summary>
    /// <param name="id">GUID do empréstimo.</param>
    /// <param name="request">Novos dados do empréstimo.</param>
    /// <returns>O empréstimo atualizado.</returns>
    /// <response code="200">Empréstimo atualizado com sucesso.</response>
    /// <response code="400">Caso as regras de validação falhem.</response>
    /// <response code="404">Caso o empréstimo com o GUID fornecido não seja encontrado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmprestimoResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] EmprestimoRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        logger.LogInformation("Iniciando atualização de empréstimo : {EmprestimoId} TraceId {TraceId}", id, traceId);

        var emprestimo = emprestimoService.Update(id, request);
        if (emprestimo is null)
        {
            logger.LogWarning("Empréstimo não encontrado para atualização : {EmprestimoId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        logger.LogInformation("Finalizando atualização de empréstimo : {EmprestimoId} TraceId {TraceId}", id, traceId);

        return Ok(emprestimo);
    }

    /// <summary>
    /// Exclui um empréstimo da base de dados.
    /// </summary>
    /// <param name="id">GUID do empréstimo a ser excluído.</param>
    /// <returns>Sem conteúdo (204 NoContent).</returns>
    /// <response code="204">Empréstimo excluído com sucesso.</response>
    /// <response code="404">Caso o empréstimo com o GUID informado não seja encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var traceId = HttpContext.TraceIdentifier;

        logger.LogInformation("Iniciando exclusão de empréstimo : {EmprestimoId} TraceId {TraceId}", id, traceId);

        if (!emprestimoService.Delete(id))
        {
            logger.LogWarning("Empréstimo não encontrado para exclusão : {EmprestimoId} TraceId {TraceId}", id, traceId);
            return NotFound();
        }

        logger.LogInformation("Finalizando exclusão de empréstimo : {EmprestimoId} TraceId {TraceId}", id, traceId);

        return NoContent();
    }
}
