using BibliotecaELM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliotecaELM.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("PG_Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.NomeUsuario)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(u => u.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Cpf)
            .HasMaxLength(11)
            .IsRequired();

        // Salt (backing field da auto-property)
        builder.Property("Salt")
            .HasColumnName("Salt")
            .HasMaxLength(100)
            .IsRequired();

        //1..N
        builder.HasMany(u => u.Emprestimos)
            .WithOne()
            .HasForeignKey(r => r.Usuario)
            .OnDelete(DeleteBehavior.Cascade);

        //1..1
        builder.HasMany(u => u.Compras)
            .WithOne()
            .HasForeignKey(c => c.Usuario)
            .OnDelete(DeleteBehavior.Cascade);
    }
}