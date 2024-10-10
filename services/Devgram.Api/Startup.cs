using Devgram.Api.Configuration;
using Devgram.Api.Interfaces;
using Devgram.Auth.Configuration;

namespace Devgram.Api
{
    public class Startup : IStartupApplication
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(hostEnvironment.ContentRootPath)
             .AddJsonFile("appsettings.json", true, true)
             .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
             .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var conn = Environment.GetEnvironmentVariable("DB_CONN") ?? Configuration.GetConnectionString("DefaultConnection");
            services.AddEndpointsApiExplorer();
            services.AddSwaggerConfig();
            services.AddAutoMapper(typeof(Startup));
            services.AddDependencyConfig();
            services.AddApiConfig(Configuration);
            services.AddIdentityConfig(Configuration);
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            app.UseSwaggerConfig();

            app.UseHttpsRedirection();
            app.UseApiConfig(env);
        }
    }

    public static class StartupExtensions
    {
        public static WebApplicationBuilder UseStartup<TStartup>(this WebApplicationBuilder webApplicationBuilder) where TStartup : IStartupApplication
        {
            var startup = Activator.CreateInstance(typeof(TStartup), webApplicationBuilder.Environment) as IStartupApplication;

            if (startup == null) throw new("Classe startup.cs inválida");

            startup.ConfigureServices(webApplicationBuilder.Services);

            var app = webApplicationBuilder.Build();

            startup.Configure(app, app.Environment);

            try
            {
                app.Run();

            }
            catch (Exception ex)
            {

            }

            return webApplicationBuilder;
        }
    }
}