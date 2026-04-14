using BibliotecaELM.Application.Services;
using BibliotecaELM.Infrastructure.Repositories;
using BibliotecaELM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaELM;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<BibliotecaElmContext>(options =>
        {
            // Conexão com o Oracle
            options.UseOracle(builder.Configuration.GetConnectionString("BibliotecaElmOracle"));
        });

        builder.Services.AddOpenApi();
        
        builder.Services.AddScoped<IAutorRepository, AutorRepository>();
        builder.Services.AddScoped<ILivroRepository, LivroRepository>();
        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddScoped<ICompraRepository, CompraRepository>();
        builder.Services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();
        builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();

        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
