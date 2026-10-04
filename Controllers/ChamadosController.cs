using DeskFlow.API.Services;
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
private readonly IChamadoService _chamadoService;

public ChamadosController(
    DeskFlowContext context,
    IChamadoService chamadoService)
{
    _context = context;
    _chamadoService = chamadoService;
}

    // GET: api/Chamados?status=1&prioridade=2&categoriaId=5
[HttpGet]
public async Task<ActionResult<IEnumerable<Chamado>>> GetChamados(
    [FromQuery] StatusChamado? status,
    [FromQuery] Prioridade? prioridade,
    [FromQuery] int? categoriaId)
{
    var query = _context.Chamados
        .Include(c => c.Categoria)
        .AsQueryable();

    if (status.HasValue)
    {
        query = query.Where(c => c.Status == status.Value);
    }

    if (prioridade.HasValue)
    {
        query = query.Where(c => c.Prioridade == prioridade.Value);
    }

    if (categoriaId.HasValue)
    {
        query = query.Where(c => c.CategoriaId == categoriaId.Value);
    }

    return await query.ToListAsync();
}

    // GET: api/Chamados/5
[HttpGet("{id}")]
public async Task<ActionResult<Chamado>> GetChamado(int id)
{
    var chamado = await _context.Chamados
        .Include(c => c.Categoria)
        .Include(c => c.Interacoes)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (chamado == null)
    {
        return NotFound("Chamado não encontrado.");
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

// POST: api/Chamados/{id}/iniciar
[HttpPost("{id}/iniciar")]
public async Task<IActionResult> IniciarChamado(int id)
{
    var sucesso = await _chamadoService.IniciarChamadoAsync(id);

    if (!sucesso)
    {
        return BadRequest(
            "Chamado não encontrado ou não está aberto.");
    }

    return NoContent();
}

// POST: api/Chamados/{id}/encerrar
[HttpPost("{id}/encerrar")]
public async Task<IActionResult> EncerrarChamado(
    int id,
    [FromBody] string solucao)
{
    var chamado = await _context.Chamados.FindAsync(id);

    if (chamado == null)
    {
        return NotFound("Chamado não encontrado.");
    }

    if (chamado.Status != StatusChamado.EmAndamento)
    {
        return BadRequest("Somente chamados em andamento podem ser encerrados.");
    }

    if (string.IsNullOrWhiteSpace(solucao))
    {
        return BadRequest("A solução do chamado é obrigatória.");
    }

    var sucesso = await _chamadoService.EncerrarChamadoAsync(id, solucao);

    if (!sucesso)
    {
        return BadRequest("Não foi possível encerrar o chamado.");
    }

    return NoContent();
}
[HttpPost("{id}/interacoes")]
public async Task<IActionResult> AdicionarInteracao(
    int id,
    [FromBody] Interacao interacao)
{
    var chamado = await _context.Chamados.FindAsync(id);

    if (chamado == null)
    {
        return NotFound("Chamado não encontrado.");
    }

    if (chamado.Status == StatusChamado.Fechado)
    {
        return BadRequest("Não é possível adicionar interações em chamados fechados.");
    }

    if (string.IsNullOrWhiteSpace(interacao.Autor) ||
        string.IsNullOrWhiteSpace(interacao.Mensagem))
    {
        return BadRequest("Autor e mensagem são obrigatórios.");
    }

    interacao.ChamadoId = id;
    interacao.DataRegistro = DateTime.UtcNow;

    _context.Interacoes.Add(interacao);

    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetChamado),
        new { id = chamado.Id },
        interacao);
}
}