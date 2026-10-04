using BibliotecaELM.Application.Services.Implementations;
using BibliotecaELM.Application.Services.Interfaces;
using BibliotecaELM.Infrastructure.Repositories;

namespace BibliotecaELM.API.Extensions;

/// <summary>
/// Extensões para registro de persistência, repositórios e serviços de aplicação na injeção de dependências.
/// Segue o padrão de separação e organização visto no projeto Recommenda.
/// </summary>
public static class BibliotecaElmServiceCollectionExtensions
{
    /// <summary>
    /// Registra os repositórios da aplicação no contêiner de DI.
    /// </summary>
    public static IServiceCollection AddBibliotecaElmRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IAutorRepository, AutorRepository>();
        services.AddScoped<ILivroRepository, LivroRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ICompraRepository, CompraRepository>();
        services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();

        return services;
    }

    /// <summary>
    /// Registra os serviços da camada Application no contêiner de DI.
    /// </summary>
    public static IServiceCollection AddBibliotecaElmApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAutorService, AutorService>();
        services.AddScoped<ILivroService, LivroService>();
        services.AddScoped<ILivroAppService, LivroAppService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<ICompraService, CompraService>();
        services.AddScoped<IEmprestimoService, EmprestimoService>();
        services.AddScoped<IEnderecoService, EnderecoService>();

        return services;
    }
}
