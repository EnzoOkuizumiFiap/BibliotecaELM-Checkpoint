using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmprestimoController : ControllerBase
{
    private readonly IEmprestimoRepository _emprestimoRepository;

    public EmprestimoController(IEmprestimoRepository emprestimoRepository)
    {
        _emprestimoRepository = emprestimoRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var emprestimos = _emprestimoRepository.GetAll();
        return Ok(emprestimos);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var emprestimo = _emprestimoRepository.GetById(id);
        if (emprestimo is null)
            return NotFound();

        return Ok(emprestimo);
    }

    [HttpPost]
    public IActionResult Create([FromBody] EmprestimoRequest request)
    {
        try
        {
            var emprestimo = _emprestimoRepository.Create(request);
            return Ok(emprestimo);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        if (!_emprestimoRepository.Delete(id))
            return NotFound();

        return NoContent();
    }
}
