using Asp.Versioning;

namespace BibliotecaELM.API.Extensions;

/// <summary>
/// Extensões para configuração do versionamento de API com Asp.Versioning.
/// Segue a parametrização do projeto Recommenda e os requisitos do CP5.
/// </summary>
public static class VersioningExtensions
{
    public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(2, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new QueryStringApiVersionReader("api-version"),
                new HeaderApiVersionReader("X-Api-Version")
            );
        })
        .AddMvc()
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVVV"; // v1.0, v2.0
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
