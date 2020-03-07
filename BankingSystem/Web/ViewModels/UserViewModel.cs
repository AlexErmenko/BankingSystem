using System.Collections.Generic;
using Infrastructure.Identity;

namespace Web.ViewModels
{
	public class UserViewModel
	{
		public object  User { get; set; }
		public List<ApplicationUser> AppUsers { get; set; }
		public List<ApplicationUser> ManagerUsers { get; set; }
		
	}
}
