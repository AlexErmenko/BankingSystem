using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Web.Commands;
using Web.ViewModels.Credit;

namespace Web.Controllers
{
  /// <summary>
  ///     Оформление, жизненный цикл кредитов
  /// </summary>
  [Authorize(Roles = "Client, Manager")]
  public class CreditController : Controller
  {
    private readonly IAsyncRepository<Credit> _creditRepository;
    private readonly IMediator _mediator;
    private IBankAccountRepository _bankAccountRepository;
    private IAsyncRepository<Currency> _currencyRepository;

    public CreditController(IAsyncRepository<Credit> creditRepository, IMediator mediator)
    {
      _creditRepository = creditRepository;
      _mediator = mediator;
    }

    /// <summary>
    ///     Оформление кредита пользователем или менеджером
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> TakeCreditForm(int idAccount) => View(model: new TakeCreditViewModel
    {
      IdAccount = idAccount
    });

    [HttpPost]
    public async Task<IActionResult> TakeCreditForm(TakeCreditViewModel takeCreditViewModel) => View();

    /// <summary>
    ///     Получение всех кредитов пользователя
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> AllCredits(int? idClient)
    {
      if(idClient == null) idClient = await _mediator.Send(request: new GetUserByIdQuery(Login: User.Identity.Name));

      IEnumerable<Credit> data = _creditRepository.GetAll().Result.Where(predicate: c => c.IdAccountNavigation.IdClient == idClient);

      return View(model: new AllCreditClientViewModel
      {
        Credits = data,
        IdClient = idClient
      });
    }

    public async Task<IActionResult> Index() => View();
  }
}
