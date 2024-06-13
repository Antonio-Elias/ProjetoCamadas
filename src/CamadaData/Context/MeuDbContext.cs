using CamadaBusiness.Models;
using Microsoft.EntityFrameworkCore;

namespace CamadaData.Context;

public class MeuDbContext : DbContext
{
    public MeuDbContext(DbContextOptions options) : base(options)  
    {
        
    }

    public DbSet<Produto> Produtos {  get; set; }
    public DbSet<Endereco> Enderecos{ get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuDbContext).Assembly);

        // configuração para tirar o Delete Cascade.
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        base.OnModelCreating(modelBuilder);
    }
}
