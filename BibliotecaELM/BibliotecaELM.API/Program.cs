using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Application.Services.Implementations;
using BibliotecaELM.Infrastructure.Repositories;
using BibliotecaELM.Infrastructure.Persistence;
using BibliotecaELM.API.Exceptions;
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

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
            {
                Title = "BibliotecaELM API",
                Version = "v1",
                Description = "API REST de gerenciamento de Biblioteca desenvolvida para o Checkpoint 3 (FIAP). Permite o controle de usuários, endereços, autores, livros, compras e empréstimos."
            });

            var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        builder.Services.AddScoped<IAutorRepository, AutorRepository>();
        builder.Services.AddScoped<ILivroRepository, LivroRepository>();
        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddScoped<ICompraRepository, CompraRepository>();
        builder.Services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();
        builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();

        builder.Services.AddScoped<IAutorService, AutorService>();
        builder.Services.AddScoped<ILivroService, LivroService>();
        builder.Services.AddScoped<IUsuarioService, UsuarioService>();
        builder.Services.AddScoped<ICompraService, CompraService>();
        builder.Services.AddScoped<IEmprestimoService, EmprestimoService>();
        builder.Services.AddScoped<IEnderecoService, EnderecoService>();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseExceptionHandler();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BibliotecaELM API v1");
                c.RoutePrefix = string.Empty; // Serve na raiz (http://localhost:port/)
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
