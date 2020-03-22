using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Commands;
using Web.ViewModels.Credit;

namespace Web.Controllers
{
	/// <summary>
	/// Оформление, жизненный цикл кредитов
	/// </summary>
	[Authorize(Roles = "Client, Manager")]
	public class CreditController : Controller
	{
		private readonly IAsyncRepository<Credit> _creditRepository;
		private IBankAccountRepository _bankAccountRepository;
		private IAsyncRepository<Currency> _currencyRepository;
		private readonly IMediator _mediator;

		public CreditController(IAsyncRepository<Credit> creditRepository, IBankAccountRepository bankAccountRepository, 
								IMediator mediator)
		{
			_creditRepository = creditRepository;
			_mediator = mediator;
			_bankAccountRepository = bankAccountRepository;
		}

		/// <summary>
		/// Оформление кредита пользователем или менеджером
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> TakeCreditForm(int idAccount)
		{
			var accounts = _bankAccountRepository.Accounts.Where(p => p.AccountType == "кредитный");

			return View(new TakeCreditViewModel()
			{
				IdAccount = idAccount,
				BankAccounts = accounts
			});
		}

		[HttpPost]
		public async Task<IActionResult> TakeCreditForm(TakeCreditViewModel takeCreditViewModel)
		{
			return View();
		}

		/// <summary>
		/// Получение всех кредитов пользователя
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> AllCredits(int? idClient)
		{
			if (idClient == null)
			{
				idClient = await _mediator.Send(request: new GetUserByIdQuery(Login: User.Identity.Name));
			}

			var data = _creditRepository.GetAll().Result
										.Where(c => c.IdAccountNavigation.IdClient == idClient);

			return View(new AllCreditClientViewModel()
			{
				Credits = data,
				IdClient = idClient
			});
		}

		public async Task<IActionResult> Index()
		{
			return View();
		}
	}
}