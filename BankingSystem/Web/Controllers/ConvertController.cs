using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ConvertController : Controller
	{
		// GET
		[HttpGet("{id}")]
		public string Index(int id)
		{

			return $"{id}";
		}
	}
}
