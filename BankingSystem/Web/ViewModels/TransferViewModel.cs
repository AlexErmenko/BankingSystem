using System;

namespace Web.Controllers
{
	public class TransferViewModel
	{
		public int IdFrom { get; set; }
		public int IdTo { get; set; }
		public decimal Amount { get; set; }
	}
}
