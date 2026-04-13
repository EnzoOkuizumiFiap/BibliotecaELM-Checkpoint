using BibliotecaELM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliotecaELM.Infrastructure.Persistence.Configurations;

public class LivroConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.ToTable("PG_Books");

        builder.HasKey(l => l.Id);
        
        builder.Property(l => l.NomeLivro)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(l => l.Preco)
            .HasPrecision(10, 2)
            .IsRequired();
        
        builder.Property(l => l.DataLancamento)
            .IsRequired();

        // 1 Livro tem 1 Autor, e 1 Autor tem Vários Livros
        builder.HasOne(l => l.Autor)
            .WithMany(a => a.Livros)
            .IsRequired();

        // N Livros N Compras
        builder.HasMany(l => l.Compras)
            .WithMany(c => c.Livros);
        
        // N Livros N Emprestimos
        builder.HasMany(l => l.Emprestimos)
            .WithMany(e => e.Livros);
    }
}