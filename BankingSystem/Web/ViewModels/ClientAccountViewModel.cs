using System;

namespace Web.ViewModels
{
	public class ClientAccountViewModel
	{
		public string AccountType { get; set; }
		public decimal Amount { get; set; }
		public string Currency { get; set; }
		public DateTime? DateClose { get; set; }
		public DateTime DateOpen { get; set; }
		public int Id { get; set; }
	}
}
