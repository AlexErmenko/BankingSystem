using System;

namespace Web.Commands
{
	public class AccountOperationViewModel
	{
		public decimal  Amount        { get; set; }
		public string   Type          { get; set; }
		public DateTime OperationTime { get; set; }
	}
}