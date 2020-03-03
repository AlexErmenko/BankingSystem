using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	public class CurrenciesController : Controller
	{
		public IAsyncRepository<Currency> Repository { get; set; }

		public CurrenciesController(IAsyncRepository<Currency> repository) { Repository = repository; }


		// GET
		public IActionResult Index() { return View(); }

	}
}