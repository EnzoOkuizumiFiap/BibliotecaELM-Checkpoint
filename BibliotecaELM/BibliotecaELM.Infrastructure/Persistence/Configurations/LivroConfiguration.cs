using BibliotecaELM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliotecaELM.Infrastructure.Persistence.Configurations;

public class LivroConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.ToTable("BD_Books");

        builder.HasKey(l => l.Id);
        
        builder.Property(l => l.NomeLivro)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(l => l.Preco)
            .HasPrecision(10, 2)
            .IsRequired();
        
        builder.Property(l => l.DataLancamento)
            .IsRequired();

        builder.HasIndex(l => new { l.AutorId });

        // Mapeamento N:N com Compra
        builder.HasMany(l => l.Compras)
            .WithMany(c => c.Livros)
            .UsingEntity(j => j.ToTable("BD_PurchaseBooks"));
        
        // Mapeamento N:N com Emprestimo
        builder.HasMany(l => l.Emprestimos)
            .WithMany(e => e.Livros)
            .UsingEntity(j => j.ToTable("BD_LoanBooks"));
    }
}