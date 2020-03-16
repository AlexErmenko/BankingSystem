using System;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Web.Commands;
using Web.ViewModels;

namespace Web.Controllers
{
	/// <summary>
	///     Просмотр операций по счёту
	/// </summary>
	public class OperationController : Controller
	{
		private readonly IMediator Mediator;
		public IAsyncRepository<BankAccount> Repository { get; set; }
		public OperationController(IMediator mediator, IAsyncRepository<BankAccount> Repository)
		{
			Mediator = mediator;
			this.Repository = Repository;
		}

		// GET
		public async Task<IActionResult> Index(int id, AccountOperationViewModel formModel)
		{
			var viewModel = await Mediator.Send(request: new GetAccountOperationQuery(id: id, startPeriod: formModel.StartPeriod, endPeriod: formModel.EndPeriod));
			return View(model: viewModel);
		}

		[HttpGet]
		public IActionResult Transfer()
		{
			var transferViewModel = new TransferViewModel();
			return View(transferViewModel);
		}


		[HttpPost]
		public async Task<IActionResult> Transfer([FromForm] TransferViewModel TransferViewModel)
		{
			// var fromAccount = await Repository.GetById(TransferViewModel.IdFrom);
			// var toAccount = await Repository.GetById(TransferViewModel.IdTo);
			//
			// if(fromAccount.Amount < TransferViewModel.Amount) { throw new NotImplementedException(); }
			//
			// fromAccount.Amount -= TransferViewModel.Amount;
			// toAccount.Amount += TransferViewModel.Amount;
			//
			// await Repository.UpdateAsync(fromAccount);
			//
			// await Repository.UpdateAsync(toAccount);
			var result = await Mediator.Send(new TransferAmountCommand(TransferViewModel.IdFrom, TransferViewModel.IdTo, TransferViewModel.Amount));

			return LocalRedirect("~/BankAccount/GetAccounts");
		}
	}
}
