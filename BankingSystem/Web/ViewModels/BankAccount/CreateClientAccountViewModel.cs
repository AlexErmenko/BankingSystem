using System;

using ApplicationCore.Entity;

namespace Web.ViewModels.BankAccount
{
	public class CreateClientAccountViewModel
	{
		public PhysicalPerson PhysicalPerson { get; set; }
		public LegalPerson LegalPerson { get; set; }
		public ApplicationCore.Entity.BankAccount Account { get; set; }
		public string ReturnUrl { get; set; } = "/";
	}
}
