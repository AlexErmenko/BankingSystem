﻿using System;
using System.ComponentModel.DataAnnotations;

using ApplicationCore.Entity;

namespace Web.ViewModels
{
	public class ClientCreateViewModel
	{
		public Client Client { get; set; }

		[Required, DataType(dataType: DataType.Password), Display(Name = "Пароль")]
		public string Password { get; set; }

		[Required, Compare(otherProperty: "Password", ErrorMessage = "Пароли не совпадают!"), DataType(dataType: DataType.Password), Display(Name = "Подтвердить пароль")]
		public string PasswordConfirm { get; set; }

		[Required, Display(Name = "Email"), DataType(dataType: DataType.EmailAddress)]
		public string Email { get; set; }

		[Required, Display(Name = "Тип Субъекта")]
		public bool IsPhysicalPerson { get; set; }
	}
}
