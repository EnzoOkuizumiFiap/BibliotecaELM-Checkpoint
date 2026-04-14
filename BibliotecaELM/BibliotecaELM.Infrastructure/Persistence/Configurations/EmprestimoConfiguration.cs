using BibliotecaELM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliotecaELM.Infrastructure.Persistence.Configurations;

public class EmprestimoConfiguration
{
    public void Configure(EntityTypeBuilder<Emprestimo> builder)
    {
        builder.ToTable("PG_Loan");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.DataEmprestimo)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(c => c.DataDevolucao)
            .HasColumnType("date")
            .IsRequired();

        builder.HasIndex(e => new { e.UsuarioId })
            .IsUnique();
    }
}