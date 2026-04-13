using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AutorController : ControllerBase
{
    private readonly IAutorRepository _autorRepository;

    public AutorController(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var autores = _autorRepository.GetAll();
        return Ok(autores);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var autor = _autorRepository.GetById(id);
        if (autor is null)
            return NotFound();

        return Ok(autor);
    }

    [HttpPost]
    public IActionResult Create([FromBody] AutorRequest request)
    {
        try
        {
            var autor = _autorRepository.Create(request);
            return Ok(autor);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        if (!_autorRepository.Delete(id))
            return NotFound();

        return NoContent();
    }
}
