using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entity;
using ApplicationCore.Specifications;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

using Xunit;

namespace xUnitLib
{
	class AdminControllerTest
	{
		UserManager<ApplicationUser> _userManager;
		AdminControllerTest(UserManager<ApplicationUser> userManager) { _userManager = userManager; }
		public ApplicationUser Inid()
		{
			var manager = new ApplicationUser
			{
				UserName    = "11",
				PhoneNumber = "24214",
				Email       = "sdqdq@sdqqd.com",
			};
			_userManager.CreateAsync(user: manager, password: AuthorizationConstants.DEFAULT_PASSWORD);
			_userManager.AddToRoleAsync(user: manager, role: AuthorizationConstants.Roles.MANAGER); 
			
			return manager;
		}
		public ApplicationUser GetManager()
		{
			return new ApplicationUser() {Id = "195797ec-5482-41a3-af1f-e86c450c4d6d", UserName = "31321452",Email = "sdqsqdqs142@gmail.com"};
		}
	}
}
