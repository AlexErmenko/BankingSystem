using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationCore.Entity;

namespace ApplicationCore.Interfaces
{
	/// <summary>
	/// Интерфейс репозитория счетов клиентов
	/// </summary>
	public interface IBankAccountRepository
	{
		IQueryable<BankAccount> Accounts { get; }

		void SaveAccount(BankAccount account);
		void DeleteAccount(int       accountId);
		void CloseAccount(int        idAccount);
		void RemoveAccount(int       idAccount);
	}
}
