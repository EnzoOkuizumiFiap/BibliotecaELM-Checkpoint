using Microsoft.EntityFrameworkCore;
using BibliotecaELM.Domain.Entities;


namespace BibliotecaELM.Infrastructure.Persistence;

public class BibliotecaELMContext(DbContextOptions<BibliotecaELMContext> options) : DbContext(options)
{
    
    public DbSet<Autor> Autor { get; set; }
    
    public DbSet<Compra> Compra { get; set; }
    
    public DbSet<Emprestimo> Emprestimo { get; set; }
    
    public DbSet<Endereco> Endereco { get; set; }
    
    public DbSet<Livro> Livro { get; set; }
    
    public DbSet<Usuario> Usuario { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BibliotecaELMContext).Assembly);
    }
}