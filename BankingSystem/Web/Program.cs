using System;
using System.Threading.Tasks;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
			var host = CreateHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope())
			{
				var services      = scope.ServiceProvider;
				var loggerFactory = services.GetRequiredService<ILoggerFactory>();
				try
				{
					// var catalogContext = services.GetRequiredService<BankingSystemContext>();
					//await BankingSystemContextSeed.SeedAsync(catalogContext, loggerFactory);

					var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
					var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
					await ApplicationDbContextSeed.SeedAsync(userManager, roleManager);
				}
				catch (Exception ex)
				{
					var logger = loggerFactory.CreateLogger<Program>();
					logger.LogError(ex, "An error occurred seeding the DB.");
				}
			}

			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
					   .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
			//TODO: Uncomment service on Prod
			//Configure background task
			.ConfigureServices(services =>
			{
				services.AddHostedService<ConsumeScopedServiceHostedService>();
				services.AddScoped<IScopedСurrencyService, ScopedСurrencyService>();
			});
		}
	}
}