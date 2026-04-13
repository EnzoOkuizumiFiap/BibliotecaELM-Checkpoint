using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var usuarios = _usuarioRepository.GetAll();
        return Ok(usuarios);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var usuario = _usuarioRepository.GetById(id);
        if (usuario is null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpPost]
    public IActionResult Create([FromBody] UsuarioRequest request)
    {
        try
        {
            var usuario = _usuarioRepository.Create(request);
            return Ok(usuario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        if (!_usuarioRepository.Delete(id))
            return NotFound();

        return NoContent();
    }
}
