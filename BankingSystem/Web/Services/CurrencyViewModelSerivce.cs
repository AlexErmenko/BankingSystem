using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Web.Commands;
using Web.ViewModels;

namespace Web.Services
{
	public class CurrencyViewModelService : ICurrencyViewModelService
	{
		private readonly IAsyncRepository<BankAccount>     _bankAccountRepository;
		private readonly IAsyncRepository<Client>          _clientRepository;
		private readonly IAsyncRepository<Currency>        _currencyRepository;
		private          ILogger<CurrencyViewModelService> Logger   { get; }
		private IMediator                         Mediator { get; }

		public CurrencyViewModelService(IAsyncRepository<BankAccount>     bankAccountRepository,
										IAsyncRepository<Client>          clientRepository,
										IAsyncRepository<Currency>        currencyRepository,
										ILogger<CurrencyViewModelService> logger, IMediator mediator)
		{
			_bankAccountRepository = bankAccountRepository;
			_clientRepository      = clientRepository;
			_currencyRepository    = currencyRepository;
			Logger                 = logger;
			Mediator               = mediator;
		}

		public async Task<List<CurrencyViewModel>> GetCurrencyRate()
		{
			Logger.LogInformation($"{nameof(GetCurrencyRate)} called");

			var specification = new CurrencyWithRateSpecification();
			var listCurrency = await _currencyRepository
									 .ListAsync(specification)
									 .ConfigureAwait(continueOnCapturedContext: true);
			var list = (from currency in listCurrency
						let lastUpdate = currency.ExchangeRates.Last()
						select new CurrencyViewModel
						{
							Id       = currency.Id,
							Name     = currency.ShortName,
							BuyRate  = lastUpdate.RateBuy,
							SaleRate = lastUpdate.RateSale
						}).ToList();

			Logger.LogInformation($"Was returned {list.Count} currency view models.");
			return list;
		}

		[HttpGet]
		public async Task<IEnumerable<ClientAccountViewModel>> GetClientAccounts(string login)
		{
			var clients = await _clientRepository.GetAll();
			var first   = clients.FirstOrDefault(client => client.Login.Equals(login));

			var bankaccounts = await _bankAccountRepository.GetAll();
			var currencies   = await _currencyRepository.GetAll();

			// foreach (var account in bankaccounts.Where(account => account.IdClient == first.Id))
			// first.BankAccounts.Add(account);

			// foreach (var account in first.BankAccounts)
			// {
			// var firstOrDefault = currencies.FirstOrDefault(currency => currency.Id == account.IdCurrency);
			// account.IdCurrencyNavigation = firstOrDefault;
			// }

			var clientAccountViewModels = first.BankAccounts.Select(it => new ClientAccountViewModel
			{
				Id          = it.Id,
				AccountType = it.AccountType,
				Amount      = it.Amount,
				DateOpen    = it.DateOpen,
				DateClose   = it.DateClose,
				Currency    = it.IdCurrencyNavigation.ShortName
			});

			return clientAccountViewModels;
		}

		public async Task<EditBankAccountViewModel> GetBankAccountViewModel(int id)
		{
			var account    = await _bankAccountRepository.GetById(id);
			var currencies = await _currencyRepository.GetAll();
			var viewModel = new EditBankAccountViewModel
			{
				SelectCurrencyList = new SelectList(currencies, "Id", "ShortName"),
				ConvertModel = new CurrencyConvertModel
				{
					From = account.IdCurrencyNavigation
								  .ShortName,
					FromId = account.IdCurrency,
					Amount = account.Amount
				}
			};
			return viewModel;
		}

		public async Task<GetCurrencyConvertQuery> GetConvertQuery(int accountId, int currencyId)
		{
			var account    = await _bankAccountRepository.GetById(accountId);
			var toCurrency = await _currencyRepository.GetById(currencyId);
			var currencies = await _currencyRepository.GetAll();


			var fromCurrencyName = account.IdCurrencyNavigation.ShortName;
			var toCurrencyName   = toCurrency.ShortName;
			var balance          = account.Amount;

			var convertQuery = new GetCurrencyConvertQuery(fromCurrencyName, toCurrencyName, balance);
			return convertQuery;
		}

		public async Task ChangeAccountCurrency(int accountId, int currencyId, decimal balance)
		{
			var account = await _bankAccountRepository.GetById(accountId);
			account.Amount     = balance;
			account.IdCurrency = currencyId;
			await _bankAccountRepository.UpdateAsync(account);
		}
	}
}
