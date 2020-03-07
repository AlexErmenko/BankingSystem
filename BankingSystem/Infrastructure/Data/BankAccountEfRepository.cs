using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

namespace Infrastructure.Data
{
	class BankAccountEfRepository : IBankAccountRepository
	{
		private readonly BankingSystemContext _context;
		public IQueryable<BankAccount> Accounts => _context.BankAccounts;

		public BankAccountEfRepository(BankingSystemContext context)
		{
			_context = context;
		}

		public void SaveAccount(BankAccount account) { throw new NotImplementedException(); }

		public void DeleteAccount(int accountId) { throw new NotImplementedException(); }

		public void CloseAccount(int idAccount) { throw new NotImplementedException(); }

		public void RemoveAccount(int idAccount) { throw new NotImplementedException(); }
	}
}
