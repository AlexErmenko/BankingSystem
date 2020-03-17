using System;

using ApplicationCore.Specifications;

using Infrastructure.Identity;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

using Moq;

using Web.Controllers;

using Xunit;

namespace xUnitLib
{
	public class AdminControllerTest
	{
		private UserManager<ApplicationUser> _userManager;

		/*public ApplicationUser Inid()
		{
			var manager = new ApplicationUser
			{
				Id = "11",
				UserName = "11",
				PhoneNumber = "24214",
				Email = "sdqdq@sdqqd.com"
			};
			_userManager.CreateAsync(user: manager, password: AuthorizationConstants.DEFAULT_PASSWORD);
			_userManager.AddToRoleAsync(user: manager, role: AuthorizationConstants.Roles.MANAGER);

			return manager;
		}*/

		[Fact]
		public void GetManager()
		{
			//Arrange
			// var mock = new Mock<UserManager<ApplicationUser>>();
			// var m1 = new Mock<ApplicationDbContext>();
			// var m2 = new Mock<IWebHostEnvironment>();
			// mock.Setup(expression: manager => manager.FindByIdAsync("11")).ReturnsAsync(value: Inid());
			//var controller = new AdminController(userManager: _userManager, context: m1.Object, appEnvironment: m2.Object);
			//var result = controller.ManagerList();
		}
	}
}
