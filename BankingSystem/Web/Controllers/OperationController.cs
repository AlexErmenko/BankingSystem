using System;
using System.Threading.Tasks;

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

    public OperationController(IMediator mediator) => Mediator = mediator;

    // GET
    public async Task<IActionResult> Index(int id, AccountOperationViewModel formModel)
    {
      AccountOperationViewModel viewModel = await Mediator.Send(request: new GetAccountOperationQuery(id: id, startPeriod: formModel.StartPeriod, endPeriod: formModel.EndPeriod));
      return View(model: viewModel);
    }

    [HttpGet]
    public IActionResult Transfer(int id)
    {
      var transferViewModel = new TransferViewModel
      {
        IdFrom = id
      };

      return View(model: transferViewModel);
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
      bool result = await Mediator.Send(request: new TransferAmountCommand(From: TransferViewModel.IdFrom, To: TransferViewModel.IdTo, Amount: TransferViewModel.Amount));

      return LocalRedirect(localUrl: "~/BankAccount/GetAccounts");
    }
  }
}
