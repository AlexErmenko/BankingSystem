using System;
using System.Linq;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

namespace Infrastructure.Data
{
	public class BankAccountEfRepository : IBankAccountRepository
	{
		private readonly BankingSystemContext _context;

		public BankAccountEfRepository(BankingSystemContext context) => _context = context;

		public IQueryable<BankAccount> Accounts => _context.BankAccounts;

		/// <summary>
		///     Создание счета или сохранение изменений
		/// </summary>
		/// <param name="account"></param>
		public void SaveAccount(BankAccount account)
		{
			if(account.Id == 0) _context.BankAccounts.AddAsync(entity: account);
			else
			{
				var bankAccount = _context.BankAccounts.FirstOrDefault(predicate: s => s.Id == account.Id);

				if(bankAccount != null)
				{
					bankAccount.AccountType = account.AccountType;
					bankAccount.Amount = account.Amount;
					bankAccount.DateClose = account.DateClose;
					bankAccount.IdCurrency = account.IdCurrency;
				}
			}

			_context.SaveChangesAsync();
		}

		public void DeleteAccount(int idAccount)
		{
			var bankAccount = _context.BankAccounts.FirstOrDefault(predicate: account => account.Id == idAccount);

			if(bankAccount?.DateClose != null)
			{
				_context.BankAccounts.Remove(entity: bankAccount);
				_context.SaveChangesAsync();
			}
		}

		public void CloseAccount(int idAccount)
		{
			var bankAccount = _context.BankAccounts.FirstOrDefault(predicate: account => account.Id == idAccount);

			if(bankAccount != null)
			{
				if(bankAccount.DateClose == null)
				{
					bankAccount.DateClose = DateTime.Now;

					_context.SaveChangesAsync();
				}
			}
		}
	}
}
