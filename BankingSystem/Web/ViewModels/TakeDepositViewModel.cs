using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
	public class TakeDepositViewModel
	{

		public int Id { get;        set; }
		public int? IdAccount { get; set; }
		public decimal PercentDeposit { get; set; }
		public decimal Amount { get; set; }
		public DateTime StartDateDeposit { get; set; }

		public DateTime EndDateDeposit { get; set; }
		public string TypeOfDeposit { get; set; }
		public bool Status { get;          set; }
	}
}
