using Devgram.Auth.Configuration;
using Devgram.Data.Infra;
using Microsoft.EntityFrameworkCore;

namespace Devgram.Api.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfig(this IServiceCollection services, IConfiguration configuration)
        {
            // services.AddMvc(options =>
            // {
            //     options.MaxModelBindingCollectionSize = int.MaxValue;
            // });

            // var conn = Environment.GetEnvironmentVariable("DB_CONN") ?? configuration.GetConnectionString("DefaultConnection");
            var conn = configuration.GetConnectionString("DefaultConnection") ?? configuration["DB_CONN"];
            services.AddDbContext<DevgramDbContext>(options => options.UseSqlite(conn));

            services.AddControllers();

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

            services.AddSwaggerGen();
        }

        public static void UseApiConfig(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Total");

            app.UseIdentityConfig();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}