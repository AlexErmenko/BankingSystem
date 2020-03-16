using System;

using MediatR;

namespace Web.Handlers
{
	public class ClientAccountOperationCommand : IRequest
	{
		public ClientAccountOperationCommand(int IdAccount, string Type, decimal Amount)
		{
			this.IdAccount = IdAccount;
			this.Type = Type;
			this.Amount = Amount;
		}

		public int IdAccount { get; }
		public string Type { get; }
		public decimal Amount { get; }
	}
}
