using System;

using ApplicationCore.Interfaces;

using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Identity;

using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.V8;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using React.AspNet;

using Web.Services;

namespace Web
{
	//с папочки Web открываем консоль 
	//dotnet tool install --global dotnet-ef
	//dotnet restore
	//dotnet tool restore
	//Restore AppCore

	//Не забываем поставить свой сервер в appsettings
	//dotnet ef database update -c bankingsystemcontext  -p ../Infrastructure/Infrastructure.csproj -s Web.csproj
	//Restore Identity
	//dotnet ef database update -c applicationdbcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj

	//Коменда для добавление миграции для осн. БД. 
	//dotnet ef migrations add InitMigration --context bankingsystemcontext --project ../Infrastructure/Infrastructure.csproj --startup-project Web.csproj

	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration) => Configuration = configuration;

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			CreateIdentityIfNotCreated(services: services);

			services.AddScoped(serviceType: typeof(IAsyncRepository<>), implementationType: typeof(EfRepository<>));

			services.AddTransient(serviceType: typeof(IBankAccountRepository), implementationType: typeof(BankAccountEfRepository));
			services.AddScoped<CurrencyViewModelService>();
			services.AddScoped<ICurrencyViewModelService, CurrencyViewModelService>();

			services.AddDbContext<ApplicationDbContext>(optionsAction: options => options.UseSqlServer(connectionString: Configuration.GetConnectionString(name: "DefaultConnection")));
			services.AddDbContext<BankingSystemContext>(optionsAction: options => options.UseSqlServer(connectionString: Configuration.GetConnectionString(name: "DomainConnection")));

			services.AddControllersWithViews();

			services.AddRazorPages();

			services.AddMemoryCache();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddReact();
			services.AddJsEngineSwitcher(configure: options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName).AddV8();

			// services.AddSwaggerGen(it => it.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"}));
		}

		private static void CreateIdentityIfNotCreated(IServiceCollection services)
		{
			var sp = services.BuildServiceProvider();
			using(var scope = sp.CreateScope())
			{
				var existingUserManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
				if(existingUserManager == null) services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
			}
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if(env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			} else
			{
				app.UseExceptionHandler(errorHandlingPath: "/Home/Error");

				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			// app.UseDefaultFiles();
			app.UseReact(configure: it => { });

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			/*app.UseSwagger();

			app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
			*/

			app.UseEndpoints(configure: endpoints =>
			{
				endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

				endpoints.MapRazorPages();
			});
		}
	}
}
