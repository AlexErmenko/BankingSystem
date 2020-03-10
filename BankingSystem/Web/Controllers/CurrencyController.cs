using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Web.Services;

namespace Web.Controllers
{
	/// <summary>
	///     Контроллер для работы с валютой
	/// </summary>
	public class CurrencyController : Controller
	{
		private  ICurrencyViewModelService _currencyViewModelSerivce;

		public CurrencyController(ICurrencyViewModelService currencyViewModelSerivce) => _currencyViewModelSerivce = currencyViewModelSerivce;

		/// <summary>
		///     Просмотре текущего курса валют
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public async Task<IActionResult> GetInfo()
		{
			var currencyRate = await _currencyViewModelSerivce.GetCurrencyRate();

			return View(model: currencyRate);
		}

		// GET !TODO: Раскоментить
		// [Authorize(Roles = AuthorizationConstants.Roles.CLIENT)]
		public async Task<IActionResult> Index()
		{
			var clientAccountViewModels = await _currencyViewModelSerivce.GetClientAccounts(id: 3);

			return View(model: clientAccountViewModels);
		}
	}
}
