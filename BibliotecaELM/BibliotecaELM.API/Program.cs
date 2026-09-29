using System.Reflection;
using Asp.Versioning;
using BibliotecaELM.API.Exceptions;
using BibliotecaELM.API.Extensions;
using BibliotecaELM.API.Health;
using BibliotecaELM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaELM.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ─── Banco de Dados ──────────────────────────────────────────────────────────
        builder.Services.AddDbContext<BibliotecaElmContext>(options =>
        {
            options.UseOracle(
                builder.Configuration.GetConnectionString("BibliotecaElmOracle"),
                oracleOptions => oracleOptions.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19));
        });

        // ─── Health Checks (CP4) ──────────────────────────────────────────────────────
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<BibliotecaElmContext>(name: "database");

        // ─── Injeção de Dependências (Repositories & Application Services) ───────────
        builder.Services.AddBibliotecaElmRepositories();
        builder.Services.AddBibliotecaElmApplicationServices();

        // ─── Versionamento de API (CP5) ───────────────────────────────────────────────
        builder.Services.AddApiVersioningConfiguration();

        // ─── Rate Limiting Nativo (CP5) ───────────────────────────────────────────────
        builder.Services.AddApiRateLimiting();

        // ─── Controllers & API Explorer ──────────────────────────────────────────────
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // ─── Swagger / OpenAPI (CP5) ──────────────────────────────────────────────────
        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }
        });

        // ─── Tratamento Global de Exceções & ProblemDetails ──────────────────────────
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        var app = builder.Build();

        // ─── Pipeline HTTP ────────────────────────────────────────────────────────────
        // 1. Exception Handler — primeiro middleware para capturar qualquer falha subsequente
        app.UseExceptionHandler();

        // 2. Rate Limiter — antes de MapControllers e Swagger
        app.UseRateLimiter();

        // 3. Swagger em Development (com dropdown dinâmico por versão descoberta)
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in app.DescribeApiVersions())
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        $"BibliotecaELM API {description.GroupName}");
                }
                options.RoutePrefix = string.Empty; // Swagger UI na raiz (http://localhost:<port>/)
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // 4. Endpoint de Health Check — isento do rate limit via DisableRateLimiting()
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
        }).DisableRateLimiting();

        // 5. Mapeamento de Controllers
        app.MapControllers();

        app.Run();
    }
}
