using BibliotecaELM.Application.DTOs;
using BibliotecaELM.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaELM.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompraController : ControllerBase
{
    private readonly ICompraRepository _compraRepository;

    public CompraController(ICompraRepository compraRepository)
    {
        _compraRepository = compraRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var compras = _compraRepository.GetAll();
        return Ok(compras);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var compra = _compraRepository.GetById(id);
        if (compra is null)
            return NotFound();

        return Ok(compra);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CompraRequest request)
    {
        try
        {
            var compra = _compraRepository.Create(request);
            return Ok(compra);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] CompraRequest request)
    {
        try
        {
            var compra = _compraRepository.Update(id, request);
            if (compra is null)
                return NotFound();

            return Ok(compra);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        if (!_compraRepository.Delete(id))
            return NotFound();

        return NoContent();
    }
}
