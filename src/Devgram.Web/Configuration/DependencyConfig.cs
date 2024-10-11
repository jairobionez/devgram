using Devgram.Auth.Extensions;
using Devgram.Data.Infra;
using Devgram.Data.Interfaces;

namespace Devgram.Web.Configuration;

public static class DependencyConfig
{
    public static IServiceCollection AddDependencyConfig(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<UsuarioRepository>();
        services.AddScoped<PublicacaoRepository>();
        services.AddScoped<IAspnetUser, AspnetUser>();
        services.AddScoped<Notifiable>();

        return services;
    }
}