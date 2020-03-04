using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Web.Models;

namespace Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController>    logger, IAsyncRepository<ExchangeRate> exchangeRepository,
							  IAsyncRepository<Currency> repository)
		{
			_logger            = logger;
			ExchangeRepository = exchangeRepository;
			CurrencyRepository = repository;
		}

		public IAsyncRepository<Currency>     CurrencyRepository { get; set; }
		public IAsyncRepository<ExchangeRate> ExchangeRepository { get; set; }

		public async Task<IActionResult> Index()
		{
			var dtos = MyMethod();

			var list = await CurrencyRepository.GetAll();

			foreach (var dto in dtos)
			{
				var currencies = list.FirstOrDefault(it => it.ShortName.Equals(dto.ShortName.Trim()));
				if (currencies != null) { dto.Id = currencies.IdCurrency; }
			}

			foreach (var item in dtos)
			{
				Console.WriteLine(item.Id);
				Console.WriteLine(item.ShortName);
				Console.WriteLine(item.Buy);
				Console.WriteLine(item.Sale);
			}


			return View();
		}

		public IActionResult Privacy() { return View(); }


		public List<CurrencyDto> MyMethod()
		{
			var Json = "";

			using (var webClient = new WebClient())
			{
				Json = webClient.DownloadString("https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5");
			}

			var jArray = JArray.Parse(Json);

			var l = new List<CurrencyDto>();


			foreach (var item in jArray)
			{
				l.Add(new CurrencyDto
				{
					ShortName = (string) item["ccy"],
					Buy       = (decimal) item["buy"],
					Sale      = (decimal) item["sale"]
				});
			}

			// l.ForEach(s => Console.WriteLine($"Currency - {s.ShortName} | Buy - {s.Buy} | Sale - {s.Sale}"));
			return l;
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
		}
	}
}