using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Newtonsoft.Json.Linq;

namespace Web.Services
{
	/// <summary>
	///     Implementation background task  logic
	/// </summary>
	public class ScopedСurrencyService : IScopedСurrencyService
	{
		private readonly IAsyncRepository<Currency>     _currencyRepository;
		private readonly IAsyncRepository<ExchangeRate> _exchangeRepository;
		private          List<Currency>                 _list;


		public ScopedСurrencyService(IAsyncRepository<Currency>     currencyRepository,
									 IAsyncRepository<ExchangeRate> exchangeRepository)
		{
			_currencyRepository = currencyRepository;
			_exchangeRepository = exchangeRepository;
		}


		public async Task DoWork(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				var array = await LoadJson().ConfigureAwait(continueOnCapturedContext: true);

				var exchangeRates = await _exchangeRepository.GetAll();


				var dtos = array.Select(it => new CurrencyDto
				{
					ShortName = (string) it["ccy"],
					Buy       = (decimal) it["buy"],
					Sale      = (decimal) it["sale"]
				}).ToList();


				foreach (var dto in dtos)
				{
					var currency                 = _list.FirstOrDefault(it => it.ShortName.Equals(dto.ShortName));
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

				// !TODO : Implement save to data base value!!
				//Delay to repeat 1 hour
				await Task.Delay(millisecondsDelay: 3_600_000, stoppingToken);
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
			_list = await _currencyRepository.GetAll().ConfigureAwait(continueOnCapturedContext: true);
			var jArray = JArray.Parse(response);

			return jArray;
		}
	}
}