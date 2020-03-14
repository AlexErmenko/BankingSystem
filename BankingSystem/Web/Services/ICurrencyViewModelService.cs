using System.Collections.Generic;
using System.Threading.Tasks;

using Web.ViewModels;

namespace Web.Services
{
	public interface ICurrencyViewModelService
	{
		Task<List<CurrencyViewModel>> GetCurrencyRate();
		Task<IEnumerable<ClientAccountViewModel>> GetClientAccounts(int id);
	}
}
