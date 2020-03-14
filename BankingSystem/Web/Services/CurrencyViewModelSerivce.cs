using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Web.ViewModels;

namespace Web.Services
{

	public class CurrencyViewModelService : ICurrencyViewModelService
	{
		private readonly IAsyncRepository<BankAccount> _bankAccountRepository;
		private readonly IAsyncRepository<Client> _clientRepository;
		private readonly IAsyncRepository<Currency> _currencyRepository;

		private ILogger<CurrencyViewModelService> Logger { get; }

		public CurrencyViewModelService(IAsyncRepository<Currency> currencyRepository, IAsyncRepository<Client> clientRepository, ILogger<CurrencyViewModelService> logger, IAsyncRepository<BankAccount> bankAccountRepository)
		{
			_currencyRepository = currencyRepository;
			_clientRepository = clientRepository;
			Logger = logger;
			_bankAccountRepository = bankAccountRepository;
		}

		public async Task<List<CurrencyViewModel>> GetCurrencyRate()
		{
			Logger.LogInformation(message: $"{nameof(GetCurrencyRate)} called");

			var specification = new CurrencyWithRateSpecification();
			var listCurrency = await _currencyRepository.ListAsync(specification: specification).ConfigureAwait(continueOnCapturedContext: true);
			var list = ( from currency in listCurrency
						 let lastUpdate = currency.ExchangeRates.Last()
						 select new CurrencyViewModel
						 {
							 Id = currency.Id,
							 Name = currency.ShortName,
							 BuyRate = lastUpdate.RateBuy,
							 SaleRate = lastUpdate.RateSale
						 } ).ToList();

			Logger.LogInformation(message: $"Was returned {list.Count} currency view models.");
			return list;
		}

		[HttpGet]
		public async Task<IEnumerable<ClientAccountViewModel>> GetClientAccounts(int id)
		{
			var first = await _clientRepository.GetById(id: id);
			var bankaccounts = await _bankAccountRepository.GetAll();
			var currencies = await _currencyRepository.GetAll();

			// foreach (var account in bankaccounts.Where(account => account.IdClient == first.Id))
			// first.BankAccounts.Add(account);

			// foreach (var account in first.BankAccounts)
			// {
			// var firstOrDefault = currencies.FirstOrDefault(currency => currency.Id == account.IdCurrency);
			// account.IdCurrencyNavigation = firstOrDefault;
			// }

			var clientAccountViewModels = first.BankAccounts.Select(selector: it => new ClientAccountViewModel
			{
				Id = it.Id,
				AccountType = it.AccountType,
				Amount = it.Amount,
				DateOpen = it.DateOpen,
				DateClose = it.DateClose,
				Currency = it.IdCurrencyNavigation.ShortName
			});

			return clientAccountViewModels;
		}
	}
}
