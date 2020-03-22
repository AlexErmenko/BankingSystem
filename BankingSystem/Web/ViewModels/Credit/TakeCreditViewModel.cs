using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels.Credit
{
	public class TakeCreditViewModel
	{
		public int IdAccount { get; set; }
		public IEnumerable<ApplicationCore.Entity.BankAccount> BankAccounts { get; set; }
		public ApplicationCore.Entity.Credit Credit { get; set; }
	}
}
