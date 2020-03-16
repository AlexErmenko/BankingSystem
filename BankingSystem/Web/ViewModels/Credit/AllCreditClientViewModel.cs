using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels.Credit
{
	/// <summary>
	/// Для получения всех кредитов пользователя
	/// </summary>
	public class AllCreditClientViewModel
	{
		public int IdClient { get; set; }
		public IEnumerable<ApplicationCore.Entity.Credit> Credits { get; set; }
	}
}
