using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReceitaWsExtractor.Domain.Entities;

namespace ReceitaWsExtractor.Infra.Configuration;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Client");
        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Surname).IsRequired();
        builder.Property(c => c.Email).IsRequired();
        builder.Property(c => c.CreatedIn).IsRequired();
        builder.OwnsMany(c => c.Orders, o =>
        {
            o.ToTable("Orders");
            o.WithOwner().HasForeignKey("ClientId");
        });
        builder.OwnsOne(c => c.Token, t =>
        {
            t.ToTable("Token");
            t.WithOwner().HasForeignKey("ClientId");
        });
    }
}
