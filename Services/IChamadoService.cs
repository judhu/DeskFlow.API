using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Services;

public interface IChamadoService
{
    Task<bool> IniciarChamadoAsync(int id);

    Task<bool> EncerrarChamadoAsync(int id, string solucao);
}