using BibliotecaELM.Exceptions;
using BibliotecaELM.Application.Services.Implementations;
using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Infrastructure.Persistence;
using BibliotecaELM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

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
        
        // Configuração dos Health Checks
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy("API operando sem problemas."))
            .AddDbContextCheck<BibliotecaElmContext>(
                name: "database",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "oracle" }
            );

        // Permite acessar o HttpContext (usado para rastrear o TraceId na camada de Application)
        builder.Services.AddHttpContextAccessor();

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

        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = report.Status == HealthStatus.Unhealthy
                    ? StatusCodes.Status503ServiceUnavailable
                    : StatusCodes.Status200OK;

                var isDev = app.Environment.IsDevelopment();

                var response = new
                {
                    status = report.Status.ToString(),
                    duration = report.TotalDuration.ToString(),
                    checks = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        status = e.Value.Status.ToString(),
                        duration = e.Value.Duration.ToString(),
                        exception = isDev ? e.Value.Exception?.Message : null
                    })
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }));
            }
        });

        app.MapControllers();

        app.Run();
    }
}
