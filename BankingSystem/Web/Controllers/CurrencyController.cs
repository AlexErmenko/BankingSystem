using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers
{
	public class CurrencyController : Controller
	{
		private readonly CurrencyViewModelSerivce _currencyViewModelSerivce;

		public CurrencyController(CurrencyViewModelSerivce currencyViewModelSerivce)
		{
			_currencyViewModelSerivce = currencyViewModelSerivce;
		}


		public async Task<IActionResult> GetInfo()
		{
			var currencyRate = await _currencyViewModelSerivce.GetCurrencyRate();
			return View(currencyRate);
		}


		// GET
		public async Task<IActionResult> Index() { return View(); }
	}
}