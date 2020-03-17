using System;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.BankingSystemContext;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using Microsoft.EntityFrameworkCore;

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
		public async Task SaveAccount(BankAccount account)
		{
			if(account.Id == 0) await _context.BankAccounts.AddAsync(entity: account);
			else
			{
				var bankAccount = await _context.BankAccounts.FirstAsync(predicate: s => s.Id == account.Id);

				if(bankAccount != null)
				{
					bankAccount.AccountType = account.AccountType;
					bankAccount.Amount = account.Amount;
					bankAccount.DateClose = account.DateClose;
					bankAccount.IdCurrency = account.IdCurrency;
				}
			}

			await _context.SaveChangesAsync();
		}

		public async Task DeleteAccount(int idAccount)
		{
			var bankAccount = await _context.BankAccounts.FirstAsync(predicate: account => account.Id == idAccount);

			if(bankAccount?.DateClose != null)
			{
				_context.BankAccounts.Remove(entity: bankAccount);
				await _context.SaveChangesAsync();
			}
		}

		public async Task CloseAccount(int idAccount)
		{
			var bankAccount = await _context.BankAccounts.FirstAsync(predicate: account => account.Id == idAccount);

			if(bankAccount != null)
			{
				if(bankAccount.DateClose == null)
				{
					bankAccount.DateClose = DateTime.Now;

					await _context.SaveChangesAsync();
				}
			}
		}
	}
}
