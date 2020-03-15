using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Services
{
	public class EditBankAccountViewModel
	{
		public SelectList           SelectCurrencyList { get; set; }
		public CurrencyConvertModel ConvertModel       { get; set; }
	}
}