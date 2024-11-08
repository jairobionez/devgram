
using Devgram.Auth.Configuration;
using Devgram.Data.Infra;
using Devgram.Web;
using Devgram.Web.Configuration;

// Declaração do builder
var builder = WebApplication.CreateBuilder(args);

// Configurando o AppSettings.json conforme ambiente de execução
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddControllersWithViews();
builder.Services.AddDependencyConfig();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddWebConfig(builder.Configuration);
builder.Services.AddIdentityConfig(builder.Configuration);
builder.Services.DbInitializer();

var app = builder.Build();

app.UseWebConfig(app.Environment);

app.Run();