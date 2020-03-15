using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;

namespace Web.ViewModels
{
	public class LegalPersonCreateViewModel
	{
		public Client Client { get; set; }

		public string Password { get; set; }

		public string Email { get; set; }

		public LegalPerson LegalPerson { get; set; }
	}
}