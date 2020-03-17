using System;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Entity;

namespace ApplicationCore.Interfaces
{
	/// <summary>
	///     Интерфейс репозитория счетов клиентов
	/// </summary>
	public interface IBankAccountRepository
	{
		IQueryable<BankAccount> Accounts { get; }

		Task SaveAccount(BankAccount account);
		Task DeleteAccount(int idAccount);
		Task CloseAccount(int idAccount);
	}
}
