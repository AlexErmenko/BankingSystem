using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Web.Commands;
using Web.ViewModels;

namespace Web.Controllers
{
    public class DepositController : Controller
	{
		private readonly IAsyncRepository<Deposit> _deposit;
		private readonly IAsyncRepository<BankAccount> _bankaccount;
		private readonly IAsyncRepository<Credit> _credit;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IMediator Mediator;
		public DepositController(IAsyncRepository<Deposit> deposit, IAsyncRepository<BankAccount> bankaccount,
								 IHttpContextAccessor httpContextAccessor, IAsyncRepository<Credit> credit,IMediator mediator)
		{
			_deposit = deposit;
			_bankaccount = bankaccount;
			_httpContextAccessor = httpContextAccessor;
			_credit = credit;
			Mediator = mediator;
		}

		public async Task<IActionResult> DepositConditions() => View();

		private async Task<int?> GetUserId() => await Mediator.Send(request: new GetUserByIdQuery(Login: User.Identity.Name));
		//GET: TakeDeposit
		[HttpGet]
		public async Task<IActionResult> TakeDeposit()
		{
			// var account = await GetUserId();
			// var accountId = await _credit.GetById(account);
			// var deposit = new TakeDepositViewModel
			// {
			// 	IdAccount = ,
			//
			// };

			return View();
		}

		////POST: TakeDeposit
		//[HttpPost, ValidateAntiForgeryToken]
		//public async Task<IActionResult> TakeDeposit(Deposit deposit)
		//{
		//	var status = await _credit.GetById(deposit.IdAccount);
		//	var result =await  Mediator.Send(new GetClientCreditQuery(deposit.IdAccount, status.Status));
		//	if (result == null)
		//	{

		//	}
			
			
			
		//	return View();
		//}
	}
}