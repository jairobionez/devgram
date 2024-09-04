namespace Devgram.Web.Interfaces
{
    public interface IStartupApplication
    {
        IConfiguration Configuration { get; }
        void ConfigureServices(IServiceCollection services);
        void Configure(WebApplication app, IWebHostEnvironment env);
    }
}