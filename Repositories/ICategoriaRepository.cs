using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllAsync();

    Task<Categoria?> GetByIdAsync(int id);

    Task<Categoria> CreateAsync(Categoria categoria);

    Task UpdateAsync(Categoria categoria);

    Task DeleteAsync(int id);

    Task<bool> HasChamadosAsync(int categoriaId);
}
