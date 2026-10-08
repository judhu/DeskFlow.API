using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Categoria> CreateAsync(Categoria categoria)
    {
        categoria.Id = 0;

        return await _repository.CreateAsync(categoria);
    }

    public async Task<bool> UpdateAsync(
        int id,
        Categoria categoria)
    {
        var categoriaExistente =
            await _repository.GetByIdAsync(id);

        if (categoriaExistente == null)
        {
            return false;
        }

        categoriaExistente.Nome = categoria.Nome;
        categoriaExistente.Descricao = categoria.Descricao;

        await _repository.UpdateAsync(categoriaExistente);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var categoria =
            await _repository.GetByIdAsync(id);

        if (categoria == null)
        {
            return false;
        }

        var possuiChamados =
            await _repository.HasChamadosAsync(id);

        if (possuiChamados)
        {
            throw new InvalidOperationException(
                "A categoria não pode ser excluída porque possui chamados associados.");
        }

        await _repository.DeleteAsync(id);

        return true;
    }
}
