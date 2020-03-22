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
  ///     Обработка перевода денег
  /// </summary>
  public class TransferAmountHandler : IRequestHandler<TransferAmountCommand, bool>
  {
    private readonly IAsyncRepository<BankAccount> AccountRepository;

    private readonly IMediator Mediator;

    public TransferAmountHandler(IAsyncRepository<BankAccount> AccountRepository, IMediator Mediator)
    {
      this.AccountRepository = AccountRepository;
      this.Mediator = Mediator;
    }

    public async Task<bool> Handle(TransferAmountCommand request, CancellationToken cancellationToken)
    {
      BankAccount fromAccount = await AccountRepository.GetById(id: request.From);
      BankAccount toAccount = await AccountRepository.GetById(id: request.To);

      if(fromAccount.Amount < request.Amount) return false;

      fromAccount.Amount -= request.Amount;
      toAccount.Amount += request.Amount;

      await AccountRepository.UpdateAsync(entity: fromAccount);
      await AccountRepository.UpdateAsync(entity: toAccount);

      await Mediator.Send(request: new ClientAccountOperationCommand(IdAccount: fromAccount.Id, Type: "Перевод", Amount: request.Amount), cancellationToken: cancellationToken);
      await Mediator.Send(request: new ClientAccountOperationCommand(IdAccount: toAccount.Id, Type: "Зачисление", Amount: request.Amount), cancellationToken: cancellationToken);

      return true;
    }
  }
}
