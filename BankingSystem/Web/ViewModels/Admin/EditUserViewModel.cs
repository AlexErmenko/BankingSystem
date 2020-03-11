using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Admin
{
	public class EditUserViewModel
	{
		public string Id { get; set; }

		[Required, Display(Name = "Имя пользователя")]
		public string UserName { get; set; }

		[Required, Display(Name = "Email"), EmailAddress]
		public string Email { get; set; }

		[Required, Display(Name = "Номер телефона"), Phone]
		public string PhoneNumber { get; set; }
	}
}
