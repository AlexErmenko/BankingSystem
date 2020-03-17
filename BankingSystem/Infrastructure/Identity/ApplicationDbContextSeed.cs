using System;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;

using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity

{
	//Initialization base role and users
	public class ApplicationDbContextSeed
	{
		public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			await roleManager.CreateAsync(role: new IdentityRole(roleName: AuthorizationConstants.Roles.ADMINISTRATORS));
			await roleManager.CreateAsync(role: new IdentityRole(roleName: AuthorizationConstants.Roles.MANAGER));
			await roleManager.CreateAsync(role: new IdentityRole(roleName: AuthorizationConstants.Roles.CLIENT));

			var defaultUserName = "demouser@gmail.com";
			var defaultUser = new ApplicationUser
			{
				UserName = defaultUserName,
				Email = "demouser@gmail.com"
			};
			await userManager.CreateAsync(user: defaultUser, password: AuthorizationConstants.DEFAULT_PASSWORD);

			//Adding admin user
			var adminUserName = "admin@gmail.com";
			var adminUser = new ApplicationUser
			{
				UserName = adminUserName,
				Email = adminUserName
			};
			await userManager.CreateAsync(user: adminUser, password: AuthorizationConstants.DEFAULT_PASSWORD);

			defaultUser = await userManager.FindByNameAsync(userName: defaultUserName);
			adminUser = await userManager.FindByNameAsync(userName: adminUserName);

			var managerUserName = "manager@gmail.com";
			var managerUser = new ApplicationUser
			{
				UserName = managerUserName,
				Email = managerUserName
			};
			await userManager.CreateAsync(user: managerUser, password: AuthorizationConstants.DEFAULT_PASSWORD);

			//Вот из за этого у нас есть админ!
			//!TODO: Мы создали этого пользователя и сохранили в БД
			//После чего мы вытягиваем его из БД но уже со сгенерированным Id
			managerUser = await userManager.FindByNameAsync(userName: managerUserName);
		}

		public static async Task ClientSeed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IAsyncRepository<Client> repository)
		{
			var clients = await repository.GetAll();
			string[] emails =
			{
				"Slowpok", "Olegovna", "Pigeon", "Termos228", "User47", "Golemchik", "GoodMusic", "TipTop", "gus123", "gus321", "TestAnton", "TestAnonim2", "Qwerty123_", "Qwerty123zz_"
			};
			var i = 0;
			foreach(var item in clients)
			{
				var user = new ApplicationUser
				{
					UserName = item.Login,
					PhoneNumber = item.TelNumber,
					Email = emails[i] + "@mail.com"
				};
				await userManager.CreateAsync(user: user, password: AuthorizationConstants.DEFAULT_PASSWORD);
				await userManager.AddToRoleAsync(user: user, role: AuthorizationConstants.Roles.CLIENT);
				i++;
			}
		}
	}
}
