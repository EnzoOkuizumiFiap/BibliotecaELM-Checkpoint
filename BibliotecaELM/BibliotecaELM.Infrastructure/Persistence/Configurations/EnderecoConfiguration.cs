using BibliotecaELM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliotecaELM.Infrastructure.Persistence.Configurations;

public class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        builder.ToTable("PG_Addresses");

        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Cep)
            .HasMaxLength(9)
            .IsRequired();

        builder.Property(e => e.Estado)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Cidade)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Bairro)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Rua)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.HasOne(e => e.Usuario)
            .WithOne(u => u.Endereco)
            .HasForeignKey<Endereco>("UsuarioId") 
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}