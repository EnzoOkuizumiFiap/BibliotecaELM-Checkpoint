using BibliotecaELM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliotecaELM.Infrastructure.Persistence.Configurations;

public class EmprestimoConfiguration : IEntityTypeConfiguration<Emprestimo>
{
    public void Configure(EntityTypeBuilder<Emprestimo> builder)
    {
        builder.ToTable("BD_Loans");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.DataEmprestimo)
            .IsRequired();

        builder.Property(e => e.DataDevolucao)
            .IsRequired();

        builder.HasOne(e => e.Usuario)
            .WithMany(u => u.Emprestimos)
            .HasForeignKey("UsuarioId") 
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); 
    }
}