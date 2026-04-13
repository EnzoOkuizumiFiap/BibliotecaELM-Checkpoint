using BibliotecaELM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliotecaELM.Infrastructure.Persistence.Configurations;

public class AutorConfiguration : IEntityTypeConfiguration<Autor>
{
    public void Configure(EntityTypeBuilder<Autor> builder)
    {
        builder.ToTable("PG_Authors");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.NomeAutor)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.Nascimento)
            .IsRequired();
    }
}