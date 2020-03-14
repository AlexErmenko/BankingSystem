using System;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Services;
using ApplicationCore;
namespace Web.Controllers
{
	/// <summary>
	///     Контроллер для работы с валютой
	/// </summary>
	public class CurrencyController : Controller
	{
		private readonly ICurrencyViewModelService _currencyViewModelSerivce;

		private IAsyncRepository<BankAccount>  _BankAccountRepository  { get; }
		private IAsyncRepository<Currency>     _currencyRepository     { get; }
		private IAsyncRepository<ExchangeRate> _exchangeRateRepository { get; }

		private int ClientId { get; set; }

		public CurrencyController(ICurrencyViewModelService      currencyViewModelSerivce,
								  IAsyncRepository<BankAccount>  bankAccountRepository,
								  IAsyncRepository<Currency>     currencyRepository,
								  IAsyncRepository<ExchangeRate> exchangeRateRepository)
		{
			_currencyViewModelSerivce = currencyViewModelSerivce;
			_BankAccountRepository    = bankAccountRepository;
			_currencyRepository       = currencyRepository;
			_exchangeRateRepository   = exchangeRateRepository;
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

		[Authorize(Roles = AuthorizationConstants.Roles.CLIENT)]
		public async Task<IActionResult> Index()
		{
			var clientAccountViewModels = await _currencyViewModelSerivce.GetClientAccounts(id: 3);
			ClientId = 3;

			return View(clientAccountViewModels);
		}

		//GET
		public async Task<IActionResult> Edit(int id)
		{
			var account       = await _BankAccountRepository.GetById(id);
			var all           = await _currencyRepository.GetAll();
			var exchangeRates = await _exchangeRateRepository.GetAll();

			ViewBag.Currencies = new SelectList(all, "Id", "ShortName");
			if (account == null) return NotFound();

			return View(account);
		}

		[HttpPost]

		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(
			int id, [Bind("Id,IdClient,IdCurrency,DateOpen,DateClose,Amount,AccountType")]
			BankAccount account)
		{
			//TODO: Добавить пересчёт баланса
			if (ModelState.IsValid)
			{
				Console.WriteLine(account);

				try { await _BankAccountRepository.UpdateAsync(account); }
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}

				return RedirectToAction(nameof(Index));
			}

			return View(account);
		}

		public IActionResult Delete() { throw new NotImplementedException(); }
	}
}
