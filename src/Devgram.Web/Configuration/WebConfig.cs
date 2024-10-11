using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Devgram.Auth.Configuration;
using Devgram.Data.Infra;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

namespace Devgram.Web.Configuration
{
    public static class WebConfig
    {
        public static void AddWebConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FormOptions>(options => { options.ValueCountLimit = int.MaxValue; });

            services.AddMvc(options => { options.MaxModelBindingCollectionSize = int.MaxValue; });

            services.AddSession();

            var conn = configuration.GetConnectionString("DefaultConnection") ?? configuration["DB_CONN"];

            services.AddDbContext<DevgramDbContext>(options => { options.UseSqlite(conn); });

            services.AddControllersWithViews()
                .AddRazorOptions(options => { options.ViewLocationFormats.Add("/Shared/Components/{2}/{0}.cshtml"); });

            services.Configure<JsonSerializerOptions>(options =>
            {
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.IncludeFields = true;
            });

            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                            .WithOrigins("http://localhost:4200")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithExposedHeaders("X-Pagination"));
            });
        }

        public static void UseWebConfig(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityConfig();

            app.UseCors("Total");
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }
    }
}