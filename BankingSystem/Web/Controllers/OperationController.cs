
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	public class OperationController : Controller
	{
		// GET
		public IActionResult Index()
		{
			return View();
		}
	}
}