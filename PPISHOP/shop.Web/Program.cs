
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using shop.Web;
using shop.Web.Data;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateWebHostBuilder(args).Build();
        RunSeeding(host);
        host.Run();
    }

    // este clase me permite alimentar mi base de datos
    private static void RunSeeding(IWebHost host)
    {
        var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
        using (var scope = scopeFactory.CreateScope())
        {
            var seeder = scope.ServiceProvider.GetService<SeedDb>();
            seeder.SeedAsync().Wait();
        }
    }

    // me permite arrancar mi proyecto
    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
}
