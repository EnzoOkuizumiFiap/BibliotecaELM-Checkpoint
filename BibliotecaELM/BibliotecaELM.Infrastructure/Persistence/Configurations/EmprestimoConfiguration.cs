using BibliotecaELM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliotecaELM.Infrastructure.Persistence.Configurations;

public class EmprestimoConfiguration : IEntityTypeConfiguration<Emprestimo>
{
    public void Configure(EntityTypeBuilder<Emprestimo> builder)
    {
        builder.ToTable("BD_Loans");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.DataEmprestimo)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(c => c.DataDevolucao)
            .HasColumnType("date")
            .IsRequired();

        builder.HasIndex(e => e.UsuarioId);

        //1..N (Usuario -> Emprestimos)
        builder.HasOne<Usuario>()
            .WithMany(u => u.Emprestimos)
            .HasForeignKey(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}