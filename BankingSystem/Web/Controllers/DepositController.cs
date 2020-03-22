using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ApplicationCore.BankingSystemContext;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Web.Commands;
using Web.ViewModels;
using Web.ViewModels.Deposit;

namespace Web.Controllers
{
    public class DepositController : Controller
	{
		private readonly IAsyncRepository<Deposit> _deposit;
		private readonly BankingSystemContext _context;
		private readonly IMediator Mediator;
		private readonly IAsyncRepository<BankAccount> _bankaccount;
		public DepositController(IAsyncRepository<Deposit> deposit, IAsyncRepository<BankAccount> bankaccount,
								 IMediator mediator,BankingSystemContext context)
		{
			_deposit = deposit;
			_context = context;
			Mediator = mediator;
			_bankaccount = bankaccount;
		}

		public async Task<IActionResult> DepositConditions() => View();

		private async Task<int?> GetUserId() => await Mediator.Send(request: new GetUserByIdQuery(Login: User.Identity.Name));
		//GET: TakeDeposit
		[HttpGet]
		public async Task<IActionResult> TakeDeposit()
		{
			var userId = await GetUserId();
			var bankaccounts = from m in _context.BankAccounts
										   where (userId == m.IdClient) && ("депозитный"== m.AccountType)
										   select m.Id;
			return View(new TakeDepositViewModel
			{
				BankAccounts = new SelectList(await bankaccounts.Distinct().ToListAsync()),
			});
		}

		//POST: TakeDeposit
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> TakeDeposit(TakeDepositViewModel deposit)
		{
			var TakeDepositVM = new Deposit
			{
				IdAccount        = deposit.IdAccount,
				StartDateDeposit = DateTime.Now,
				EndDateDeposit   = deposit.EndDateDeposit,
				TypeOfDeposit    = deposit.TypeOfDeposit,
				Amount           = deposit.Amount,
				PercentDeposit   = deposit.PercentDeposit,
				Status           = true,
			};
			if (ModelState.IsValid)
			{
				var bankaccount = await _bankaccount.GetById(deposit.IdAccount);
				if (bankaccount.Amount < deposit.Amount)
				{ 
					ModelState.AddModelError(string.Empty, errorMessage:"Недостаточно средств на счету");
					return View(deposit);
				}

				bankaccount.Amount = bankaccount.Amount - TakeDepositVM.Amount;
				await _bankaccount.UpdateAsync(bankaccount);
				await _deposit.AddAsync(TakeDepositVM);
				
				
			}


			 return View(deposit);
			
		}
	}
}