using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly DeskFlowContext _context;

    public CategoriaRepository(DeskFlowContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        return await _context.Categorias
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        return await _context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Categoria> CreateAsync(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return categoria;
    }

    public async Task UpdateAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria is null)
            return;

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasChamadosAsync(int categoriaId)
    {
        return await _context.Chamados
            .AnyAsync(c => c.CategoriaId == categoriaId);
    }
}
