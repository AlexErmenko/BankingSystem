using System;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Web.Commands;

namespace Web.Controllers
{
	public class OperationController : Controller
	{
		private IMediator Mediator { get; }
		public OperationController(IMediator mediator) => Mediator = mediator;

		// GET
		public async Task<IActionResult> Index(int id)
		{
			var viewModel = await Mediator.Send(request: new GetAccountOperationQuery(id: id));

			return View(model: viewModel);
		}
	}
}
