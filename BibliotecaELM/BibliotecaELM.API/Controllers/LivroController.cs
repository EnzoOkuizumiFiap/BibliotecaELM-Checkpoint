using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LivroController : ControllerBase
{
    private readonly ILivroRepository _livroRepository;
    
    public LivroController(ILivroRepository livroRepository)
    {
        _livroRepository = livroRepository;
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var livros = _livroRepository.GetAll();
        return Ok(livros);
    }
    
    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var livro = _livroRepository.GetById(id);
        if (livro is null)
            return NotFound();

        return Ok(livro);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] LivroRequest request)
    {
        try
        {
            var livro = _livroRepository.Create(request);
            return Ok(livro);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] LivroRequest request)
    {
        try
        {
            var livro = _livroRepository.Update(id, request);
            if (livro is null)
                return NotFound();

            return Ok(livro);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        if (!_livroRepository.Delete(id))
            return NotFound();

        return NoContent();
    }
}