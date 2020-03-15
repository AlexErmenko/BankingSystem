using System;
using System.ComponentModel.DataAnnotations;

using ApplicationCore.Entity;

namespace Web.ViewModels
{
	public class PhysicalPersonCreateViewModel
	{
		public Client Client { get; set; }

		[Required, DataType(dataType: DataType.Password), Display(Name = "Пароль")]
		public string Password { get; set; }

		public string Email { get; set; }

		public PhysicalPerson PhysicalPerson { get; set; }
	}
}
