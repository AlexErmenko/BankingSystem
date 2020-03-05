using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Web.Services
{
	//Класс в котором непосредственно выполняется действие
	internal class ScopedProcessingService : IScopedProcessingService
	{
		private readonly ILogger _logger;
		private          int     executionCount;

		public ScopedProcessingService(ILogger<ScopedProcessingService> logger,
									   IAsyncRepository<Currency>       currencyRepository,
									   IAsyncRepository<ExchangeRate>   exchangeRepository)
		{
			CurrencyRepository = currencyRepository;
			ExchangeRepository = exchangeRepository;
			_logger            = logger;
		}


		public IAsyncRepository<Currency>     CurrencyRepository { get; set; }
		public List<Currency>                 List               { get; set; }
		public IAsyncRepository<ExchangeRate> ExchangeRepository { get; set; }


		public async Task DoWork(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				var array = await LoadJson();

				var exchangeRates = await ExchangeRepository.GetAll();
				executionCount++;


				var dtos = array.Select(it => new CurrencyDto
				{
					ShortName = (string) it["ccy"],
					Buy       = (decimal) it["buy"],
					Sale      = (decimal) it["sale"]
				}).ToList();


				foreach (var dto in dtos)
				{
					var currency                 = List.FirstOrDefault(it => it.ShortName.Equals(dto.ShortName));
					if (currency != null) dto.Id = currency.Id;
				}

				try
				{
					foreach (var dto in dtos)
					{
						Console.WriteLine(dto.Id);
						Console.WriteLine(dto.ShortName);
						Console.WriteLine(dto.Buy);
						Console.WriteLine(dto.Sale);
						/*var rate = new ExchangeRate
						{
							IdCurrency = dto.Id,
							DateRate   = Convert.ToDateTime($"{DateTime.Now:dd.MM.yyyy}"),
							RateBuy    = dto.Buy,
							RateSale   = dto.Sale
						};

						await ExchangeRepository.AddAsync(rate).ConfigureAwait(continueOnCapturedContext: true);*/
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}

				executionCount++;

				_logger.LogInformation("Scoped Processing Service is working. Count: {Count}", executionCount);


				await Task.Delay(millisecondsDelay: 360_000, stoppingToken);
			}
		}

		private async Task<JArray> LoadJson()
		{
			var       response = "";
			using var client   = new WebClient();
			response =
				await
					client.DownloadStringTaskAsync(new
													   Uri("https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5"));
			List = await CurrencyRepository.GetAll().ConfigureAwait(continueOnCapturedContext: true);
			var jArray = JArray.Parse(response);

			return jArray;
		}
	}
}