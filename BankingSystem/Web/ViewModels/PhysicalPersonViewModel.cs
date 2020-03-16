using System;

using ApplicationCore.Entity;

namespace Web.ViewModels
{
	public class PhysicalPersonViewModel
	{
		public Client Client { get; set; }

		public string UserName { get; set; }

		public PhysicalPerson PhysicalPerson { get; set; }
	}
}
