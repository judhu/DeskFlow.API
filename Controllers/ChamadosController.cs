using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChamadosController : ControllerBase
{
    private readonly DeskFlowContext _context;

    public ChamadosController(DeskFlowContext context)
    {
        _context = context;
    }

    // GET: api/Chamados
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Chamado>>> GetChamados()
    {
        return await _context.Chamados
            .Include(c => c.Categoria)
            .ToListAsync();
    }

    // GET: api/Chamados/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Chamado>> GetChamado(int id)
    {
        var chamado = await _context.Chamados
            .Include(c => c.Categoria)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (chamado == null)
        {
            return NotFound();
        }

        return Ok(chamado);
    }

    // POST: api/Chamados
    [HttpPost]
    public async Task<ActionResult<Chamado>> PostChamado(Chamado chamado)
    {
        var categoriaExiste = await _context.Categorias
            .AnyAsync(c => c.Id == chamado.CategoriaId);

        if (!categoriaExiste)
        {
            return BadRequest("A categoria informada não existe.");
        }

        chamado.Id = 0;
        chamado.DataAbertura = DateTime.Now;

        _context.Chamados.Add(chamado);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetChamado),
            new { id = chamado.Id },
            chamado
        );
    }

    // PUT: api/Chamados/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutChamado(int id, Chamado chamado)
    {
        if (id != chamado.Id)
        {
            return BadRequest("O ID da URL não corresponde ao ID do chamado.");
        }

        var chamadoExistente = await _context.Chamados.FindAsync(id);

        if (chamadoExistente == null)
        {
            return NotFound();
        }

        var categoriaExiste = await _context.Categorias
            .AnyAsync(c => c.Id == chamado.CategoriaId);

        if (!categoriaExiste)
        {
            return BadRequest("A categoria informada não existe.");
        }

        chamadoExistente.Titulo = chamado.Titulo;
        chamadoExistente.Descricao = chamado.Descricao;
        chamadoExistente.Prioridade = chamado.Prioridade;
        chamadoExistente.Status = chamado.Status;
        chamadoExistente.CategoriaId = chamado.CategoriaId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Chamados/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChamado(int id)
    {
        var chamado = await _context.Chamados.FindAsync(id);

        if (chamado == null)
        {
            return NotFound();
        }

        _context.Chamados.Remove(chamado);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}