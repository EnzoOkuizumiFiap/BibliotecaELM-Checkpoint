using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BibliotecaELM.API.Extensions;

/// <summary>
/// Gera um <c>SwaggerDoc</c> para cada versão da API descoberta pelo <see cref="IApiVersionDescriptionProvider"/>.
/// Segue o padrão de referência do projeto Recommenda, marcando versões deprecadas adequadamente.
/// </summary>
public sealed class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = "📚 BibliotecaELM API",
                    Version = description.ApiVersion.ToString(),
                    Description = (description.IsDeprecated || description.ApiVersion.MajorVersion == 1)
                        ? "⚠️ [ESTA VERSÃO FOI DEPRECADA. Favor utilizar a v2.0]."
                        : "API REST para gerenciamento de biblioteca desenvolvida em Clean Architecture.",
                    Contact = new OpenApiContact
                    {
                        Name = "Equipe BibliotecaELM",
                        Email = "contato@bibliotecaelm.com.br"
                    }
                });
        }
    }
}
