using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReceitaWsExtractor.Domain.Entities;
using ReceitaWsExtractor.Infra.Configuration;

namespace ReceitaWsExtractor.Infra.Context;

public class ReceitaWsExtractorDbContext : IdentityDbContext<Client, Role, Guid>
{
    public DbSet<Client> Client => Set<Client>();

    public ReceitaWsExtractorDbContext()
    {
        //Construtor vazio para rodar o Migrations
    }

    public ReceitaWsExtractorDbContext(DbContextOptions<ReceitaWsExtractorDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Apenas para executar o Migrations

        optionsBuilder.UseSqlServer();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ClientConfiguration());
    }
}
