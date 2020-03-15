using System;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Web.Commands;
using Web.ViewModels;

namespace Web.Controllers
{
	public class OperationController : Controller
	{
		private IMediator Mediator { get; }
		public OperationController(IMediator mediator) => Mediator = mediator;

		// GET
		public async Task<IActionResult> Index(int id, AccountOperationViewModel formModel)
		{
			var viewModel = await Mediator.Send(request: new GetAccountOperationQuery(id: id, startPeriod: formModel.StartPeriod, endPeriod: formModel.EndPeriod));
			return View(model: viewModel);
		}

		
	}
}
