
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Commands;

namespace Web.Controllers
{
	public class OperationController : Controller
	{
		public OperationController(IMediator mediator) { Mediator = mediator; }

		private IMediator Mediator { get; set; }
		// GET
		public async Task<IActionResult> Index(int id)
		{
			var viewModel = await Mediator.Send(new GetAccountOperationQuery(id));

			return View(viewModel);
		}
	}
}
