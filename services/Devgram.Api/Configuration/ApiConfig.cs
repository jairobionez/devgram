using Devgram.Infra;
using Devgram.Auth.Configuration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using DbContext = Devgram.Infra.DbContext;

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
            var conn = configuration["DB_CONN"];
            services.AddDbContext<DbContext>(options => options.UseSqlServer(conn));

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                     {
                         options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                         options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                         options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                         options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                         options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                         options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                     });

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