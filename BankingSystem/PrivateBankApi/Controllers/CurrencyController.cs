using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PrivateBankApi.Controllers
{
	[ApiController,Route("api/[controller]/[action]")]
	public class CurrencyController : ControllerBase
	{


		// private readonly ILogger<CurrencyController> _logger;

		public CurrencyController(ILogger<CurrencyController> logger)
		{
			// _logger = logger;
		}

		/*[HttpGet]
		public IEnumerable<CurrencyDto> Get()
		{

		}*/
	}
}
