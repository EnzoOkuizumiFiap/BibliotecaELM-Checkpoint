using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnderecoController : ControllerBase
{
    private readonly IEnderecoRepository _enderecoRepository;

    public EnderecoController(IEnderecoRepository enderecoRepository)
    {
        _enderecoRepository = enderecoRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var enderecos = _enderecoRepository.GetAll();
        return Ok(enderecos);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var endereco = _enderecoRepository.GetById(id);
        if (endereco is null)
            return NotFound();

        return Ok(endereco);
    }

    [HttpPost("{usuarioId:guid}")]
    public IActionResult Create(Guid usuarioId, [FromBody] EnderecoRequest request)
    {
        try
        {
            var endereco = _enderecoRepository.Create(request, usuarioId);
            return Ok(endereco);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        if (!_enderecoRepository.Delete(id))
            return NotFound();

        return NoContent();
    }
}
