using System;
using System.Threading;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using MediatR;

namespace Web.Controllers
{
	public class TransferAmountHandler : IRequestHandler<TransferAmountCommand, bool>
	{
		private readonly IAsyncRepository<BankAccount> AccountRepository;

		public TransferAmountHandler(IAsyncRepository<BankAccount> AccountRepository)
		{
			this.AccountRepository = AccountRepository;
		}

		public async Task<bool> Handle(TransferAmountCommand request, CancellationToken cancellationToken)
		{
			var fromAccount = await AccountRepository.GetById(request.From);
			var toAccount = await AccountRepository.GetById(request.To);

			if(fromAccount.Amount < request.Amount) { return false; }

			fromAccount.Amount -= request.Amount;
			toAccount.Amount += request.Amount;

			await AccountRepository.UpdateAsync(fromAccount);
			// await AccountRepository.UpdateAsync(toAccount);

			return true;


		}
	}
}
