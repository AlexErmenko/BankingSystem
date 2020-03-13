using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Web.Services;

namespace Web.Controllers
{
	/// <summary>
	///     Контроллер для работы с валютой
	/// </summary>
	public class CurrencyController : Controller
	{
		private ICurrencyViewModelService _currencyViewModelSerivce;

		public IAsyncRepository<BankAccount> _BankAccountRepository { get; set; }
		public IAsyncRepository<Currency> _currencyRepository { get; set; }
		public IAsyncRepository<ExchangeRate> _exchangeRateRepository { get; set; }

		public int ClientId { get; set; }
		public CurrencyController(ICurrencyViewModelService currencyViewModelSerivce, IAsyncRepository<BankAccount> bankAccountRepository, IAsyncRepository<Currency> currencyRepository, IAsyncRepository<ExchangeRate> exchangeRateRepository)
		{
			_currencyViewModelSerivce = currencyViewModelSerivce;
			_BankAccountRepository = bankAccountRepository;
			_currencyRepository = currencyRepository;
			_exchangeRateRepository = exchangeRateRepository;
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

		// GET !TODO: Раскоментить
		// [Authorize(Roles = AuthorizationConstants.Roles.CLIENT)]
		public async Task<IActionResult> Index()
		{
			var clientAccountViewModels = await _currencyViewModelSerivce.GetClientAccounts(id: 3);
			ClientId = 3;

			return View(model: clientAccountViewModels);
		}

		//GET
		public async Task<IActionResult> Edit(int id)
		{
			var account = await _BankAccountRepository.GetById(id);
			List<Currency> all = await _currencyRepository.GetAll();
			var exchangeRates = await _exchangeRateRepository.GetAll();

			ViewBag.Currencies = new SelectList(all, "Id", "ShortName");
			if(account == null) { return NotFound(); }
			return View(account);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,IdClient,IdCurrency,DateOpen,DateClose,Amount,AccountType")] BankAccount account)
		{
			//TODO: Добавить пересчёт баланса
			if(ModelState.IsValid)
			{
				Console.WriteLine(account);

				try { await _BankAccountRepository.UpdateAsync(account); } catch(Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
				return RedirectToAction(actionName: nameof(Index));
			}

			return View(account);
		}

		public IActionResult Delete() { throw new NotImplementedException(); }
	}
}
