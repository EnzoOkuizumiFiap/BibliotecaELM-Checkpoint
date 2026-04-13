using BibliotecaELM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliotecaELM.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("BD_Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.NomeUsuario)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(u => u.Nascimento)
            .IsRequired();
        
        builder.Property(u => u.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Cpf)
            .HasMaxLength(11)
            .IsRequired();
    }
}