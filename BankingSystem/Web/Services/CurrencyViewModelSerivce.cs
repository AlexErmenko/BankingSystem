using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.Extensions.Logging;
using Web.ViewModels;

namespace Web.Services
{
	public class CurrencyViewModelSerivce
	{
		private readonly IAsyncRepository<Currency> _currencyRepository;

		public CurrencyViewModelSerivce(IAsyncRepository<Currency> repository, ILogger<CurrencyViewModelSerivce> logger)
		{
			_currencyRepository = repository;
			Logger              = logger;
		}

		private ILogger<CurrencyViewModelSerivce> Logger { get; }

		public async Task<List<CurrencyViewModel>> GetCurrencyRate()
		{
			Logger.LogInformation($"{nameof(GetCurrencyRate)} called");

			var specification = new CurrencyWithRateSpecification();
			var listCurrency  = await _currencyRepository.ListAsync(specification);
			var list          = new List<CurrencyViewModel>();

			foreach (var currency in listCurrency)
			{
				var lastUpdate = currency.ExchangeRates.Last();
				var viewModel = new CurrencyViewModel
				{
					Id       = currency.Id,
					Name     = currency.ShortName,
					BuyRate  = lastUpdate.RateBuy,
					SaleRate = lastUpdate.RateSale
				};
				list.Add(viewModel);
			}

			Logger.LogInformation($"Was returned {list.Count} currency view models.");
			return list;
		}
	}
}