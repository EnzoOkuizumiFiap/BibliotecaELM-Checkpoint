using Microsoft.EntityFrameworkCore;
using BibliotecaELM.Domain.Entities;


namespace BibliotecaELM.Infrastructure.Persistence;

public class BibliotecaElmContext(DbContextOptions<BibliotecaElmContext> options) : DbContext(options)
{
    
    public DbSet<Autor> Autores { get; set; }
    
    public DbSet<Compra> Compras { get; set; }
    
    public DbSet<Emprestimo> Emprestimos { get; set; }
    
    public DbSet<Endereco> Enderecos { get; set; }
    
    public DbSet<Livro> Livros { get; set; }
    
    public DbSet<Usuario> Usuarios { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BibliotecaElmContext).Assembly);
    }
}