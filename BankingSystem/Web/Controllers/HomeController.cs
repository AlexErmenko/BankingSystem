using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Web.Models;

namespace Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger) => _logger = logger;

		public async Task<IActionResult> Index() => View();

		public IActionResult Privacy() => View();

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() => View(model: new ErrorViewModel
		{
			RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
		});
	}
}
