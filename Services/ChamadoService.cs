using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Services;

public class ChamadoService : IChamadoService
{
    private readonly DeskFlowContext _context;

    public ChamadoService(DeskFlowContext context)
    {
        _context = context;
    }

    public async Task<bool> IniciarChamadoAsync(int id)
    {
        var chamado = await _context.Chamados.FindAsync(id);

        if (chamado == null)
{
    throw new KeyNotFoundException("Chamado não encontrado.");
}

if (chamado.Status != StatusChamado.Aberto)
{
    throw new InvalidOperationException(
        "O chamado precisa estar aberto para ser iniciado.");
}

        chamado.Status = StatusChamado.EmAndamento;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EncerrarChamadoAsync(int id, string solucao)
    {
        var chamado = await _context.Chamados.FindAsync(id);

        if (chamado == null)
{
    throw new KeyNotFoundException("Chamado não encontrado.");
}

if (chamado.Status != StatusChamado.EmAndamento)
{
    throw new InvalidOperationException(
        "O chamado precisa estar em andamento para ser encerrado.");
}

if (string.IsNullOrWhiteSpace(solucao))
{
    throw new ArgumentException(
        "A solução do chamado é obrigatória.");
}

        chamado.Status = StatusChamado.Fechado;
        chamado.Solucao = solucao;
        chamado.DataFechamento = DateTime.Now;

        await _context.SaveChangesAsync();

        return true;
    }
}