using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ApplicationCore.Entity;

namespace Web.ViewModels
{
	public class AccountOperationViewModel
	{
		public int IdAccount { get; set; }
		public IList<Operation> Operations { get; set; }

		[DataType(dataType: DataType.Date)]
		public DateTime? StartPeriod { get; set; }

		[DataType(dataType: DataType.Date)]
		public DateTime? EndPeriod { get; set; }
	}
}
