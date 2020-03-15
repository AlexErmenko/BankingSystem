using ApplicationCore.BankingSystemContext;
using ApplicationCore.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Commands;
using Web.Handlers;
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
		public Startup(IConfiguration configuration) { Configuration = configuration; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			CreateIdentityIfNotCreated(services);

			services.AddMediatR(typeof(CurrencyConvertHandler), typeof(AccountOperationHandler),
								typeof(PasswordValidatorHendler));

			services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

			services.AddTransient(typeof(IBankAccountRepository),
								  typeof(BankAccountEfRepository));
			services.AddScoped<CurrencyViewModelService>();

			services.AddScoped<ICurrencyViewModelService, CurrencyViewModelService>();


			services.AddDbContext<ApplicationDbContext>(options =>
															options.UseSqlServer(Configuration
																					 .GetConnectionString("DefaultConnection")));
			services.AddDbContext<BankingSystemContext>(options =>
															options.UseSqlServer(Configuration
																					 .GetConnectionString("DomainConnection")));

			services.AddControllersWithViews();

			services.AddRazorPages();
			services.AddHttpContextAccessor();

			services.AddMemoryCache();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddDistributedMemoryCache();
			services.AddSession();

			// services.AddSwaggerGen(it => it.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"}));
		}

		private static void CreateIdentityIfNotCreated(IServiceCollection services)
		{
			var       sp                  = services.BuildServiceProvider();
			using var scope               = sp.CreateScope();
			var       existingUserManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
			if (existingUserManager == null)
				services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultUI()
						.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			} else
			{
				app.UseExceptionHandler("/Home/Error");

				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			// app.UseDefaultFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseSession();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllers();

				endpoints.MapRazorPages();
			});
		}
	}
}
