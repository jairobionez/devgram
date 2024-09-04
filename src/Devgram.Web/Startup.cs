using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Devgram.Web.Interfaces;

namespace Devgram.Web
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
            services.AddControllersWithViews();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
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