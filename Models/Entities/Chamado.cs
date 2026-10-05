using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models.Entities;

public class Chamado
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(150, ErrorMessage = "O título deve ter no máximo 150 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    public Prioridade Prioridade { get; set; }

    public StatusChamado Status { get; set; }

    [Required(ErrorMessage = "O nome do solicitante é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do solicitante deve ter no máximo 100 caracteres.")]
    public string SolicitanteNome { get; set; } = string.Empty;

    public DateTime DataAbertura { get; set; }

    public DateTime? DataFechamento { get; set; }

    public string? Solucao { get; set; }

    public int CategoriaId { get; set; }

    public Categoria? Categoria { get; set; }

    public ICollection<Interacao> Interacoes { get; set; } = new List<Interacao>();
}