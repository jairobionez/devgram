using Devgram.Auth.Extensions;
using Devgram.Infra.Interfaces;
using Devgram.Infra.Repositories;

namespace Devgram.Web.Configuration;

public static class DependencyConfig
{
    public static IServiceCollection AddDependencyConfig(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<UsuarioRepository>();
        services.AddScoped<PublicacaoRepository>();
        services.AddScoped<IAspnetUser, AspnetUser>();

        return services;
    }
}