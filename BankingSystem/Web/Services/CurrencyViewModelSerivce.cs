using System;
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
		private readonly IAsyncRepository<BankAccount> _bankAccountRepository;
		private readonly IAsyncRepository<Client> _clientRepository;
		private readonly IAsyncRepository<Currency> _currencyRepository;
		private ILogger<CurrencyViewModelService> Logger { get; }
		private IMediator Mediator { get; }

		public CurrencyViewModelService(IAsyncRepository<BankAccount> bankAccountRepository, IAsyncRepository<Client> clientRepository, IAsyncRepository<Currency> currencyRepository, ILogger<CurrencyViewModelService> logger, IMediator mediator)
		{
			_bankAccountRepository = bankAccountRepository;
			_clientRepository = clientRepository;
			_currencyRepository = currencyRepository;
			Logger = logger;
			Mediator = mediator;
		}

		/// <summary>
		///     Получение курса валют
		/// </summary>
		/// <returns></returns>
		public async Task<List<CurrencyViewModel>> GetCurrencyRate()
		{
			Logger.LogInformation(message: $"{nameof(GetCurrencyRate)} called");

			var specification = new CurrencyWithRateSpecification();

			var listCurrency = await _currencyRepository.ListAsync(specification: specification).ConfigureAwait(continueOnCapturedContext: true);

			var list = listCurrency.Select(selector: currency => new
			{
				currency,
				lastUpdate = currency.ExchangeRates.Last()
			}).Select(selector: t =>
			{
				var model = new CurrencyViewModel();
				model.Id = t.currency.Id;
				model.Name = t.currency.ShortName;
				model.BuyRate = t.lastUpdate.RateBuy;
				model.SaleRate = t.lastUpdate.RateSale;
				return model;
			}).ToList();

			Logger.LogInformation(message: $"Was returned {list.Count} currency view models.");
			return list;
		}

		/// <summary>
		///     Просмотр всех акк по клиенту
		/// </summary>
		/// <param name="login"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IEnumerable<ClientAccountViewModel>> GetClientAccounts(string login)
		{
			var clients = await _clientRepository.GetAll();
			var first = clients.FirstOrDefault(predicate: client => client.Login.Equals(value: login));

			var bankaccounts = await _bankAccountRepository.GetAll();
			var currencies = await _currencyRepository.GetAll();

			return first?.BankAccounts.Select(selector: it => new ClientAccountViewModel
			{
				Id = it.Id,
				AccountType = it.AccountType,
				Amount = it.Amount,
				DateOpen = it.DateOpen,
				DateClose = it.DateClose,
				Currency = it.IdCurrencyNavigation.ShortName
			});
		}

		/// <summary>
		///     Отображаем Edit
		/// </summary>
		/// <param name="id">Аккаунт для которого формируем VM</param>
		/// <returns></returns>
		public async Task<EditBankAccountViewModel> GetBankAccountViewModel(int id)
		{
			//Получаем Акк
			var account = await _bankAccountRepository.GetById(id: id);
			var currencies = await _currencyRepository.GetAll();

			var viewModel = new EditBankAccountViewModel
			{
				//Список с валютами
				SelectCurrencyList = new SelectList(items: currencies, dataValueField: "Id", dataTextField: "ShortName"),

				//Модель данных формы
				ConvertModel = new CurrencyConvertModel
				{
					From = account.IdCurrencyNavigation.ShortName,
					FromId = account.IdCurrency,
					Amount = account.Amount
				}
			};
			return viewModel;
		}

		/// <summary>
		///     Создаёт запрос для медиатора
		/// </summary>
		/// <param name="accountId"></param>
		/// <param name="currencyId"></param>
		/// <returns></returns>
		public async Task<GetCurrencyConvertQuery> GetConvertQuery(int accountId, int currencyId)
		{
			//Акк для которого меняем валюту
			var account = await _bankAccountRepository.GetById(id: accountId);

			//Валюта на которую меняем
			var toCurrency = await _currencyRepository.GetById(id: currencyId);

			var currencies = await _currencyRepository.GetAll();

			//Аббревиатура для API
			var fromCurrencyName = account.IdCurrencyNavigation.ShortName;
			var toCurrencyName = toCurrency.ShortName;
			var balance = account.Amount;

			var convertQuery = new GetCurrencyConvertQuery(@from: fromCurrencyName, to: toCurrencyName, amount: balance);
			return convertQuery;
		}

		/// <summary>
		///     Сохраняем изменения валюты аккаунта
		/// </summary>
		/// <param name="accountId">Аккаунт для которого меняем валюту</param>
		/// <param name="currencyId">Новая валюта</param>
		/// <param name="balance">Перерасчитанный баланс</param>
		/// <returns></returns>
		public async Task ChangeAccountCurrency(int accountId, int currencyId, decimal balance)
		{
			var account = await _bankAccountRepository.GetById(id: accountId);
			account.Amount = balance;
			account.IdCurrency = currencyId;
			await _bankAccountRepository.UpdateAsync(entity: account);
		}
	}
}
