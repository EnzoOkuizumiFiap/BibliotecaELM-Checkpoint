using BibliotecaELM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliotecaELM.Infrastructure.Persistence.Configurations;

public class CompraConfiguration : IEntityTypeConfiguration<Compra>
{
    public void Configure(EntityTypeBuilder<Compra> builder)
    {
        builder.ToTable("BD_Purchases"); 

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FormaCompra)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(c => c.DataCompra)
            .IsRequired();
        
        builder.HasIndex(e => new { e.UsuarioId })
            .IsUnique();
        
        //1..N
        builder.HasMany(c => c.Livros)
            .WithMany();
    }
}