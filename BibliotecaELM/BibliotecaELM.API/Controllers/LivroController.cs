using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Application.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

/// <summary>
/// Controller responsável por gerenciar o acervo de Livros.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class LivroController : ControllerBase
{
    private readonly ILivroService _livroService;
    
    public LivroController(ILivroService livroService)
    {
        _livroService = livroService;
    }
    
    /// <summary>
    /// Retorna todos os livros do acervo.
    /// </summary>
    /// <returns>Uma lista de livros.</returns>
    /// <response code="200">Retorna a lista de livros com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LivroResponse>))]
    public IActionResult GetAll()
    {
        var livros = _livroService.GetAll();
        return Ok(livros);
    }
    
    /// <summary>
    /// Busca um livro específico pelo GUID.
    /// </summary>
    /// <param name="id">GUID do livro.</param>
    /// <returns>Os detalhes do livro encontrado.</returns>
    /// <response code="200">Livro encontrado.</response>
    /// <response code="404">Caso não exista livro com o GUID informado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LivroResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var livro = _livroService.GetById(id);
        if (livro is null)
            return NotFound();

        return Ok(livro);
    }
    
    /// <summary>
    /// Cadastra um novo livro no acervo da biblioteca.
    /// </summary>
    /// <param name="request">Dados do livro a ser cadastrado.</param>
    /// <returns>O livro cadastrado.</returns>
    /// <response code="201">Livro cadastrado com sucesso.</response>
    /// <response code="400">Caso os dados de entrada sejam inválidos ou o autor associado não exista.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LivroResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] LivroRequest request)
    {
        var livro = _livroService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = livro.Id }, livro);
    }

    /// <summary>
    /// Atualiza os dados de um livro do acervo.
    /// </summary>
    /// <param name="id">GUID do livro a ser atualizado.</param>
    /// <param name="request">Novos dados do livro.</param>
    /// <returns>O livro atualizado.</returns>
    /// <response code="200">Livro atualizado com sucesso.</response>
    /// <response code="400">Caso os dados fornecidos falhem nas validações.</response>
    /// <response code="404">Caso o livro com o GUID informado não seja encontrado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LivroResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] LivroRequest request)
    {
        var livro = _livroService.Update(id, request);
        if (livro is null)
            return NotFound();

        return Ok(livro);
    }
    
    /// <summary>
    /// Exclui um livro do acervo.
    /// </summary>
    /// <param name="id">GUID do livro a ser removido.</param>
    /// <returns>Sem conteúdo (204 NoContent).</returns>
    /// <response code="204">Livro deletado com sucesso.</response>
    /// <response code="404">Caso o livro com o GUID fornecido não exista.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        if (!_livroService.Delete(id))
            return NotFound();

        return NoContent();
    }
}
