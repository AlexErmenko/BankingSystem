using System;

using ApplicationCore.Entity;

namespace Web.ViewModels
{
	public class LegalPersonViewModel
	{
		public Client Client { get; set; }

		public string UserName { get; set; }

		public LegalPerson LegalPerson { get; set; }
	}
}
