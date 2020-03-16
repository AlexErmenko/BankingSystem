using System;

using ApplicationCore.Entity;

namespace Web.ViewModels
{
	public class LegalPersonCreateViewModel
	{
		public Client Client { get; set; }

		public string Email { get; set; }

		public LegalPerson LegalPerson { get; set; }
	}
}
