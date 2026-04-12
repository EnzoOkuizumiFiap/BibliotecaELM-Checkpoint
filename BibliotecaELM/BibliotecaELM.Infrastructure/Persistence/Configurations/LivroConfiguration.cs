using BibliotecaELM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliotecaELM.Infrastructure.Persistence.Configurations;

public class LivroConfiguration
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.ToTable("PG_Books");

        builder.HasKey(l => l.Id);
        
        builder.Property(l => l.NomeLivro)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(l => l.Preco)
            .IsRequired();
        
        builder.Property(l => l.DataLancamento)
            .HasColumnType("date")
            .IsRequired();

        builder.HasOne(l => l.Autor)
            .WithMany(a => a.Livros)
            .IsRequired();

        builder.HasMany(l => l.Compras)
            .WithMany(c => c.Livros);
        
        builder.HasMany(l => l.Emprestimos)
            .WithMany(e => e.Livros);
    }
}