using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
	public class ApplicationUser : IdentityUser
	{
		[Required, Display(Name = "Имя пользователя")]
		public override string UserName { get; set; }

		//[Required]
		[Display(Name = "Email"), EmailAddress]
		public override string Email { get; set; }

		//[Required]
		[Display(Name = "Номер телефона"), Phone]
		public override string PhoneNumber { get; set; }
	}
}
