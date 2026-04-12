using BibliotecaELM.Application.Services;
using BibliotecaELM.Infrastructure;
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
        
        builder.Services.AddScoped<ILivroRepository, LivroRepository>();

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
