using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Web.ViewModels
{
	public class ClientCreateViewModel
	{
		public ApplicationCore.Entity.Client Client { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "Пароли не совпадают!")]
		[DataType(DataType.Password)]
		[Display(Name = "Подтвердить пароль")]
		public string PasswordConfirm { get; set; }

		[Required]
		[Display(Name = "Email")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Тип Субъекта")]
		public bool IsPhysicalPerson { get; set; }
	}
}