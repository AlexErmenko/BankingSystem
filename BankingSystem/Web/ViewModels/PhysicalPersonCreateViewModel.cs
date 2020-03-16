using System;

using ApplicationCore.Entity;

namespace Web.ViewModels
{
	public class PhysicalPersonCreateViewModel
	{
		public Client Client { get; set; }

		public string Email { get; set; }

		public PhysicalPerson PhysicalPerson { get; set; }
	}
}
