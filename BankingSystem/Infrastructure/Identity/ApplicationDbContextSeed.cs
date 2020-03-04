using System.Threading.Tasks;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity

{
	//Init data to Identity
	public class ApplicationDbContextSeed
	{
		public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
										   RoleManager<IdentityRole>    roleManager)
		{
			//Adding roles
			await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.Roles.ADMINISTRATORS));
			await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.Roles.MANAGER));
			await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.Roles.CLIENT));
			//Adding default user
			var defaultUser = new ApplicationUser {UserName = "demouser@gmail.com", Email = "demouser@gmail.com"};
			await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);
			//Adding admin user
			var adminUserName = "admin@gmail.com";
			var adminUser     = new ApplicationUser {UserName = adminUserName, Email = adminUserName};
			await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
			adminUser = await userManager.FindByNameAsync(adminUserName);
			//Adding manager
			var managerUserName = "manager@gmail.com";
			var managerUser     = new ApplicationUser {UserName = managerUserName, Email = managerUserName};
			await userManager.CreateAsync(managerUser, AuthorizationConstants.DEFAULT_PASSWORD);

			await userManager.AddToRoleAsync(defaultUser, AuthorizationConstants.Roles.CLIENT);
			await userManager.AddToRoleAsync(managerUser, AuthorizationConstants.Roles.MANAGER);
			await userManager.AddToRoleAsync(adminUser, AuthorizationConstants.Roles.ADMINISTRATORS);
		}
	}
}