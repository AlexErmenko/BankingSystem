﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;

namespace Web.ViewModels
{
	public class PhysicalPersonCreateViewModel
	{
		public Client Client { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		public string Email { get; set; }

		public PhysicalPerson PhysicalPerson { get; set; }
	}
}