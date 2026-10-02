using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Services;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriasController(ICategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categorias = await _service.GetAllAsync();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _service.GetByIdAsync(id);

        if (categoria is null)
            return NotFound();

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Categoria categoria)
    {
        var criada = await _service.CreateAsync(categoria);

        return CreatedAtAction(
            nameof(GetById),
            new { id = criada.Id },
            criada);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] Categoria categoria)
    {
        var atualizada = await _service.UpdateAsync(id, categoria);

        if (!atualizada)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var removida = await _service.DeleteAsync(id);

        if (!removida)
            return NotFound();

        return NoContent();
    }
}