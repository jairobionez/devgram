
using System.Text;
using Devgram.Web.ConfigModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Devgram.Web.Configuration
{
  public static class IdentityConfig
	{
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
        {
            // var identityBuilder = services.AddIdentity<Usuario, IdentityRole<Guid>>(options =>
            // {
            //     options.Tokens.ProviderMap["Telefone"] = new TokenProviderDescriptor(typeof(FourDigitPhoneNumberTokenProvider<Usuario>));
            //     options.Tokens.ProviderMap["Email"] = new TokenProviderDescriptor(typeof(FourDigitEmailTokenProvider<Usuario>));
            // });

            // identityBuilder.AddErrorDescriber<IdentityMensagensPortugues>()
            //     .AddEntityFrameworkStores<Context>();
            //     // .AddDefaultTokenProviders();

            // services.Configure<IdentityOptions>(options =>
            // {
            //     options.User.RequireUniqueEmail = false;
            //     options.SignIn.RequireConfirmedEmail = false;
            // });

            // services.Configure<DataProtectionTokenProviderOptions>(options =>
            // {
            //     options.TokenLifespan = TimeSpan.FromMinutes(5);
            // });

            // AddCookieAuth(services);
            // // AddJwtAuth(services, configuration);

            return services;
        }

        public static IApplicationBuilder UseIdentityConfig(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        private static void AddCookieAuth(IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/AccessDenied";
            });
        }

        private static void AddJwtAuth(IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSetting>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSetting>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}