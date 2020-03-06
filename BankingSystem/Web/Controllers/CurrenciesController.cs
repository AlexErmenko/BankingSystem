using System;
using System.Linq;
using System.Net;
using ApplicationCore.Dto;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Web.Controllers
{
	public class CurrenciesController : Controller
	{
		public CurrenciesController(IAsyncRepository<Currency>     currencyRepository,
									IAsyncRepository<ExchangeRate> exchangeRepository)
		{
			CurrencyRepository = currencyRepository;
			ExchangeRepository = exchangeRepository;
		}

		public IAsyncRepository<Currency>     CurrencyRepository { get; set; }
		public IAsyncRepository<ExchangeRate> ExchangeRepository { get; set; }


		public async void Handle()
		{
			var array         = LoadJson();
			var currencies    = await CurrencyRepository.GetAll();
			var exchangeRates = await ExchangeRepository.GetAll();

			var dtos = array.Select(it => new CurrencyDto
			{
				ShortName = (string) it["ccy"],
				Buy       = (decimal) it["buy"],
				Sale      = (decimal) it["sale"]
			}).ToList();


			foreach (var dto in dtos)
			{
				var currency                 = currencies.FirstOrDefault(it => it.ShortName.Equals(dto.ShortName));
				if (currency != null) dto.Id = currency.Id;
			}

			try
			{
				foreach (var dto in dtos)
				{
					var rate = new ExchangeRate
					{
						IdCurrency = dto.Id,
						DateRate   = Convert.ToDateTime($"{DateTime.Now:dd.MM.yyyy}"),
						RateBuy    = dto.Buy,
						RateSale   = dto.Sale
					};

					await ExchangeRepository.AddAsync(rate).ConfigureAwait(continueOnCapturedContext: true);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		private JArray LoadJson()
		{
			var       response = "";
			using var client   = new WebClient();
			response =
				client.DownloadString(new Uri("https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5"));
			return JArray.Parse(response);
		}

		// GET
		public IActionResult Index()
		{
			
			return View();
		}
	}
}