using System;

using MediatR;

namespace Web.Commands
{
  /// <summary>
  ///     Команда для фиксации операций с аккаунтом
  /// </summary>
  public class ClientAccountOperationCommand : IRequest
  {
    public int IdAccount { get; }
    public string Type { get; }
    public decimal Amount { get; }

    public ClientAccountOperationCommand(int IdAccount, string Type, decimal Amount)
    {
      this.IdAccount = IdAccount;
      this.Type = Type;
      this.Amount = Amount;
    }
  }
}
