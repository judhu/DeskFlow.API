namespace DeskFlow.API.Models.Entities;

public class Chamado
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public Prioridade Prioridade { get; set; }

    public StatusChamado Status { get; set; }

    public int CategoriaId { get; set; }

    public Categoria? Categoria { get; set; }

    public DateTime DataAbertura { get; set; } = DateTime.Now;
}