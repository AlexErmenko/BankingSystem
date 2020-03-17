using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Web.Commands;
using Web.ViewModels;

namespace Web.Services
{
	public interface ICurrencyViewModelService
	{
		Task<List<CurrencyViewModel>> GetCurrencyRate();
		Task<IEnumerable<ClientAccountViewModel>> GetClientAccounts(string? id);
		Task<EditBankAccountViewModel> GetBankAccountViewModel(int id);
		Task<GetCurrencyConvertQuery> GetConvertQuery(int accountId, int currencyId);

		Task ChangeAccountCurrency(int accountId, int currencyId, decimal balance);
	}
}
