﻿using System.Threading.Tasks;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity

{
	//Initialization base role and users
	public class ApplicationDbContextSeed
	{
		public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
										   RoleManager<IdentityRole>    roleManager)
		{
			await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.Roles.ADMINISTRATORS));
			await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.Roles.MANAGER));
			await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.Roles.CLIENT));

			var defaultUserName = "demouser@gmail.com";
			var defaultUser = new ApplicationUser {UserName = defaultUserName, Email = "demouser@gmail.com"};
			await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);


			//Adding admin user
			var adminUserName = "admin@gmail.com";
			var adminUser     = new ApplicationUser {UserName = adminUserName, Email = adminUserName};
			await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);


			defaultUser = await userManager.FindByNameAsync(defaultUserName);
			adminUser = await userManager.FindByNameAsync(adminUserName);

			var managerUserName = "manager@gmail.com";
			var managerUser     = new ApplicationUser {UserName = managerUserName, Email = managerUserName};
			await userManager.CreateAsync(managerUser, AuthorizationConstants.DEFAULT_PASSWORD);
			//Вот из за этого у нас есть админ!
			//!TODO: Мы создали этого пользователя и сохранили в БД
			//После чего мы вытягиваем его из БД но уже со сгенерированным Id
			managerUser = await userManager.FindByNameAsync(managerUserName);

			
			

			
		}
	}
}