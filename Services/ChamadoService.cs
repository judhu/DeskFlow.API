
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

        if (chamado == null || chamado.Status != StatusChamado.Aberto)
        {
            return false;
        }

        chamado.Status = StatusChamado.EmAndamento;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EncerrarChamadoAsync(int id, string solucao)
    {
        var chamado = await _context.Chamados.FindAsync(id);

        if (chamado == null ||
            chamado.Status != StatusChamado.EmAndamento ||
            string.IsNullOrWhiteSpace(solucao))
        {
            return false;
        }

        chamado.Status = StatusChamado.Fechado;
        chamado.Solucao = solucao;
        chamado.DataFechamento = DateTime.Now;

        await _context.SaveChangesAsync();

        return true;
    }
}
