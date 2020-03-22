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
  ///     Обработчик для фиксации операций над счетами пользователя
  /// </summary>
  public class ClientAccountOperationHandler : AsyncRequestHandler<ClientAccountOperationCommand>
  {
    private readonly IAsyncRepository<Operation> Repository;
    public ClientAccountOperationHandler(IAsyncRepository<Operation> Repository) => this.Repository = Repository;

    protected override async Task Handle(ClientAccountOperationCommand request, CancellationToken cancellationToken)
    {
      var entity = new Operation();
      entity.IdAccount = request.IdAccount;
      entity.OperationTime = DateTime.Now;
      entity.TypeOperation = request.Type;
      entity.Amount = request.Amount;
      await Repository.AddAsync(entity: entity);
    }
  }
}
