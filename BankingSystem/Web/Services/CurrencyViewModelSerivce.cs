using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Web.ViewModels;

namespace Web.Services
{
	public class CurrencyViewModelSerivce
	{
		private readonly IAsyncRepository<Currency> _currencyRepository;
		public           IAsyncRepository<Client>   _clientRepository { get; set; }

		public IAsyncRepository<BankAccount> _BankAccountRepository { get; set; }

		public CurrencyViewModelSerivce(IAsyncRepository<Currency>        currencyRepository,
										IAsyncRepository<Client>          clientRepository,
										ILogger<CurrencyViewModelSerivce> logger,
										IAsyncRepository<BankAccount>     bankAccountRepository)
		{
			_currencyRepository    = currencyRepository;
			_clientRepository      = clientRepository;
			Logger                 = logger;
			_BankAccountRepository = bankAccountRepository;
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

		public async Task<IEnumerable<ClientAccountViewModel>> GetClientAccounts(int id)
		{
			var first        = await _clientRepository.GetById(id);
			var bankaccounts = await _BankAccountRepository.GetAll();
			var currencies = await _currencyRepository.GetAll();

			// foreach (var account in bankaccounts.Where(account => account.IdClient == first.Id))
				// first.BankAccounts.Add(account);

			// foreach (var account in first.BankAccounts)
			// {
				// var firstOrDefault = currencies.FirstOrDefault(currency => currency.Id == account.IdCurrency);
				// account.IdCurrencyNavigation = firstOrDefault;
			// }

			var clientAccountViewModels = first.BankAccounts.Select(it => new ClientAccountViewModel
			{
				AccountType = it.AccountType,
				Amount      = it.Amount,
				DateOpen    = it.DateOpen,
				DateClose   = it.DateClose,
				Currency    = it.IdCurrencyNavigation.ShortName
			});

			return clientAccountViewModels;
		}
	}


	public class ClientAccountViewModel
	{
		public string    AccountType { get; set; }
		public decimal   Amount      { get; set; }
		public string    Currency    { get; set; }
		public DateTime? DateClose   { get; set; }
		public DateTime  DateOpen    { get; set; }
	}
}