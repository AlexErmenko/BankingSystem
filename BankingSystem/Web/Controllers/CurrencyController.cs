using System;
using System.Threading.Tasks;
using ApplicationCore.Specifications;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
		private UserManager<ApplicationUser>  UserManager            { get; set; }
		private IMediator                     Mediator               { get; }

		public CurrencyController(ICurrencyViewModelService     currencyViewModelSerivce,
								  UserManager<ApplicationUser>  manager, IMediator mediator)
		{
			_currencyViewModelSerivce = currencyViewModelSerivce;
			UserManager               = manager;
			Mediator                  = mediator;
		}

		/// <summary>
		///     Просмотре текущего курса валют
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public async Task<IActionResult> GetInfo()
		{
			var currencyRate = await _currencyViewModelSerivce.GetCurrencyRate();
			return View(currencyRate);
		}

		/// <summary>
		///     Вывод всех аккаунтов пользователя
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = AuthorizationConstants.Roles.CLIENT)]
		public async Task<IActionResult> Index()
		{
			var clientAccountViewModels = await _currencyViewModelSerivce.GetClientAccounts(User.Identity.Name);

			return View(clientAccountViewModels);
		}

		//GET
		public async Task<IActionResult> Edit(int id)
		{
			var viewModel = await _currencyViewModelSerivce.GetBankAccountViewModel(id);
			ViewBag.Currencies = viewModel.SelectCurrencyList;
			var currencyConvertModel = viewModel.ConvertModel;
			return View(currencyConvertModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(
			int id, [FromForm] CurrencyConvertModel convertModel)
		{
			var query = await _currencyViewModelSerivce.GetConvertQuery(id, convertModel.ToId);
			var balance = await Mediator.Send(query);
			if (ModelState.IsValid)
			{
				try
				{
					await _currencyViewModelSerivce.ChangeAccountCurrency(id, convertModel.ToId,balance);

				}

				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}

				return RedirectToAction(nameof(Index));
			}

			return View();
		}

		// public IActionResult Delete() { throw new NotImplementedException(); }
	}
}
