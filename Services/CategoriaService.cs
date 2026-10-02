
using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Services;

public class CategoriaService : ICategoriaService
{
    private readonly DeskFlowContext _context;

    public CategoriaService(DeskFlowContext context)
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

    public async Task<bool> UpdateAsync(int id, Categoria categoria)
    {
        var categoriaExistente = await _context.Categorias
            .FindAsync(id);

        if (categoriaExistente == null)
        {
            return false;
        }

        categoriaExistente.Nome = categoria.Nome;
        categoriaExistente.Descricao = categoria.Descricao;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var categoria = await _context.Categorias
            .FindAsync(id);

        if (categoria == null)
        {
            return false;
        }

        _context.Categorias.Remove(categoria);

        await _context.SaveChangesAsync();

        return true;
    }
}
