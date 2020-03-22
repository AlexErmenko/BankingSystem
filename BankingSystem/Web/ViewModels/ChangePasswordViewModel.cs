using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
	public class ChangePasswordViewModel
	{
		public string Email { get; set; }

		public string OldPassword { get; set; }

		public string NewPassword { get; set; }

		[Compare("NewPassword", ErrorMessage = "Пароли не совпадают!")]
		public string ConfirmNewPassword { get; set; }

	}
}
