using System;
using System.Threading.Tasks;

using ApplicationCore.Specifications;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Web.Services;
using Web.ViewModels;

namespace Web.Controllers
{
	/// <summary>
	///     Контроллер для работы с валютой
	/// </summary>
	public class CurrencyController : Controller
	{
		private readonly ICurrencyViewModelService _currencyViewModelSerivce;
		private IMediator Mediator { get; }

		public CurrencyController(ICurrencyViewModelService currencyViewModelSerivce, IMediator mediator)
		{
			_currencyViewModelSerivce = currencyViewModelSerivce;
			Mediator = mediator;
		}

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

		/// <summary>
		///     Вывод всех аккаунтов пользователя
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = AuthorizationConstants.Roles.CLIENT)]
		public async Task<IActionResult> Index()
		{
			var clientAccountViewModels = await _currencyViewModelSerivce.GetClientAccounts(id: User.Identity.Name);

			return View(model: clientAccountViewModels);
		}

		//GET
		public async Task<IActionResult> Edit(int id)
		{
			var viewModel = await _currencyViewModelSerivce.GetBankAccountViewModel(id: id);
			ViewBag.Currencies = viewModel.SelectCurrencyList;
			var currencyConvertModel = viewModel.ConvertModel;
			return View(model: currencyConvertModel);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [FromForm] CurrencyConvertModel convertModel)
		{
			var query = await _currencyViewModelSerivce.GetConvertQuery(accountId: id, currencyId: convertModel.ToId);
			var balance = await Mediator.Send(request: query);
			if(ModelState.IsValid)
			{
				try { await _currencyViewModelSerivce.ChangeAccountCurrency(accountId: id, currencyId: convertModel.ToId, balance: balance); } catch(Exception e)
				{
					Console.WriteLine(value: e);
					throw;
				}

				return RedirectToAction(actionName: nameof(Index));
			}

			return View();
		}

		// public IActionResult Delete() { throw new NotImplementedException(); }
	}
}
