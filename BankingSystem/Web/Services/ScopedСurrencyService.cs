using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Entity;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Web.Services
{
	/// <summary>
	///     Implementation background task  logic
	/// </summary>
	public class ScopedСurrencyService : IScopedСurrencyService
	{
		private readonly BankingSystemContext           _context;
		private readonly ILogger<ScopedСurrencyService> _logger;

		public ScopedСurrencyService(BankingSystemContext context, ILogger<ScopedСurrencyService> logger)
		{
			_context = context;
			_logger  = logger;
		}

		public async Task DoWork(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				var rate = _context.ExchangeRates.ToList().Last();
				DateTime value = Convert.ToDateTime($"{DateTime.Now:dd.MM.yyyy}");

				if (!rate.DateRate.Equals(value))
				{


					var array = await LoadJson().ConfigureAwait(continueOnCapturedContext: true);

					var dtos = array.Select(it => new CurrencyDto
					{
						ShortName = (string) it["ccy"],
						Buy       = (decimal) it["buy"],
						Sale      = (decimal) it["sale"]
					}).ToList();

					_logger.Log(LogLevel.Information, "Dtos" + dtos);

					try
					{
						_logger.Log(LogLevel.Information, "Start saving");
						foreach (var dto in dtos)
						{
							var firstOrDefault =
								_context.Currencies.FirstOrDefault(currency =>
																	   currency.ShortName.Equals(dto.ShortName));

							_logger.Log(LogLevel.Information, $"Currency {firstOrDefault}");

							var exchangeRate = new ExchangeRate
							{
								RateBuy  = dto.Buy,
								RateSale = dto.Sale,
								DateRate = Convert.ToDateTime($"{DateTime.Now:dd.MM.yyyy}")
							};

							_logger.Log(LogLevel.Information, $"Rate {exchangeRate}");


							if (firstOrDefault != null)
							{
								firstOrDefault.ExchangeRates.Add(exchangeRate);
								_context.SaveChanges();
							}
						}

						_logger.Log(LogLevel.Information, "End saving");
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						Console.WriteLine("Error from service");
						_logger.Log(LogLevel.Error, e.Message);
						throw;
					}
				}

				await Task.Delay(millisecondsDelay: 86_400_000, stoppingToken);
			}
		}

		/// <summary>
		///     Load json array with currency
		/// </summary>
		/// <returns></returns>
		private async Task<JArray> LoadJson()
		{
			using var client = new WebClient();
			var response = await
							   client.DownloadStringTaskAsync(new
																  Uri("https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5"));
			var jArray = JArray.Parse(response);

			return jArray;
		}
	}
}