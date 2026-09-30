using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Data;

public class DeskFlowContext : DbContext
{
    public DeskFlowContext(DbContextOptions<DeskFlowContext> options)
        : base(options)
    {
    }

    public DbSet<Chamado> Chamados { get; set; }

    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<Interacao> Interacoes { get; set; }
}