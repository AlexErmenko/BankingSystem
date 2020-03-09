using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
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

		[AllowAnonymous]
		public async Task<IActionResult> GetInfo()
		{
			var currencyRate = await _currencyViewModelSerivce.GetCurrencyRate();

			return View(currencyRate);
		}


		// GET
		// [Authorize(Roles = AuthorizationConstants.Roles.CLIENT)]
		public async Task<IActionResult> Index()
		{
			var clientAccountViewModels = await _currencyViewModelSerivce.GetClientAccounts(3);
			

			return View();
		}
	}
}