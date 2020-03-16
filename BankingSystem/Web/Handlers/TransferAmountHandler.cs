using System;
using System.Threading;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using MediatR;

using Web.Commands;

namespace Web.Handlers
{
	/// <summary>
	/// Обработка перевода денег
	/// </summary>
	public class TransferAmountHandler : IRequestHandler<TransferAmountCommand, bool>
	{
		private readonly IAsyncRepository<BankAccount> AccountRepository;

		private IMediator Mediator;

		public TransferAmountHandler(IAsyncRepository<BankAccount> AccountRepository, IMediator Mediator)
		{
			this.AccountRepository = AccountRepository;
			this.Mediator = Mediator;
		}

		public async Task<bool> Handle(TransferAmountCommand request, CancellationToken cancellationToken)
		{
			var fromAccount = await AccountRepository.GetById(request.From);
			var toAccount = await AccountRepository.GetById(request.To);

			if(fromAccount.Amount < request.Amount) { return false; }

			fromAccount.Amount -= request.Amount;
			toAccount.Amount += request.Amount;

			await AccountRepository.UpdateAsync(fromAccount);
			await AccountRepository.UpdateAsync(toAccount);

			await Mediator.Send(new ClientAccountOperationCommand(fromAccount.Id, "Перевод", request.Amount), cancellationToken);
			await Mediator.Send(new ClientAccountOperationCommand(toAccount.Id, "Зачисление", request.Amount), cancellationToken);

			return true;
		}
	}
}
