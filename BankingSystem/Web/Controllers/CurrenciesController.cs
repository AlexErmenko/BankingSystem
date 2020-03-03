using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	public class CurrenciesController : Controller
	{
		public CurrenciesController(IAsyncRepository<Currency> repository) { Repository = repository; }
		public IAsyncRepository<Currency> Repository { get; set; }


		// GET
		public IActionResult Index() { return View(); }
	}
}