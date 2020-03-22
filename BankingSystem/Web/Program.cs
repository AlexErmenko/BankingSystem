using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Web.Services;

namespace Web
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      IHost host = CreateHostBuilder(args: args).Build();

      using(IServiceScope scope = host.Services.CreateScope())
      {
        IServiceProvider services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
          //!TODO: Добавить среду для Cloud
          // var catalogContext = services.GetRequiredService<BankingSystemContext>();
          //await BankingSystemContextSeed.SeedAsync(catalogContext, loggerFactory);

          // var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
          // var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
          // var repository = services.GetRequiredService<IAsyncRepository<Client>>();

          // await ApplicationDbContextSeed.SeedAsync(userManager: userManager, roleManager: roleManager);
          // await ApplicationDbContextSeed.ClientSeed(userManager: userManager, roleManager: roleManager,
          // repository);
        } catch(Exception ex)
        {
          ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
          logger.LogError(exception: ex, message: "An error occurred seeding the DB.");
        }
      }

      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args: args).ConfigureWebHostDefaults(configure: webBuilder =>
      {
        webBuilder.UseStartup<Startup>();
      })

          //TODO: Uncomment service on Prod
          //Configure background task
          .ConfigureServices(configureDelegate: services =>
          {
            services.AddHostedService<ConsumeScopedServiceHostedService>();
            services.AddScoped<IScopedСurrencyService, ScopedСurrencyService>();
          });
    }
  }
}
