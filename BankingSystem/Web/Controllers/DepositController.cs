using System;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.BankingSystemContext;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Web.Commands;
using Web.ViewModels.Deposit;

namespace Web.Controllers
{
  public class DepositController : Controller
  {
    private readonly BankingSystemContext _context;
    private readonly IAsyncRepository<Deposit> _deposit;
    private readonly IMediator Mediator;

    public DepositController(IAsyncRepository<Deposit> deposit, IMediator mediator, BankingSystemContext context)
    {
      _deposit = deposit;
      _context = context;
      Mediator = mediator;
    }

    public async Task<IActionResult> DepositConditions() => View();

    private async Task<int?> GetUserId() => await Mediator.Send(request: new GetUserByIdQuery(Login: User.Identity.Name));

    //GET: TakeDeposit
    [HttpGet]
    public async Task<IActionResult> TakeDeposit()
    {
      int? userId = await GetUserId();
      IQueryable<int> bankaccounts = from m in _context.BankAccounts where userId == m.IdClient select m.Id;
      return View(model: new TakeDepositViewModel
      {
        IdAccount = userId,
        BankAccounts = new SelectList(items: await bankaccounts.Distinct().ToListAsync()),
        StartDateDeposit = DateTime.Now
      });
    }

    //POST: TakeDeposit
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> TakeDeposit(TakeDepositViewModel deposit) => View(model: deposit);
  }
}
