using Devgram.Infra.Repositories;

namespace Devgram.Web.Configuration;

public static class DependencyConfig
{
    public static IServiceCollection AddDependencyConfig(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<UsuarioRepository>();

        return services;
    }
}