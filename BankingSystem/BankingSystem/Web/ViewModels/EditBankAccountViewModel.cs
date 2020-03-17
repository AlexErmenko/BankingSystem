using System;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels
{
	public class EditBankAccountViewModel
	{
		public SelectList SelectCurrencyList { get; set; }
		public CurrencyConvertModel ConvertModel { get; set; }
	}
}
