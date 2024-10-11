using Devgram.Auth.Extensions;
using Devgram.Data.Infra;
using Devgram.Data.Interfaces;

namespace Devgram.Web.Configuration;

public static class DependencyConfig
{
    public static IServiceCollection AddDependencyConfig(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IPublicacaoRepository, PublicacaoRepository>();
        services.AddScoped<IAspnetUser, AspnetUser>();
        services.AddScoped<INotifiable, Notifiable>();

        return services;
    }
}