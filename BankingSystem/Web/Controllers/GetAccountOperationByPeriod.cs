using System;
using System.Collections.Generic;

using MediatR;

using Web.Commands;

namespace Web.Controllers
{
	public class GetAccountOperationByPeriod : IRequest<List<AccountOperationViewModel>>
	{
		public int AccountId { get; set; }

		public GetAccountOperationByPeriod(int AccountId) => this.AccountId = AccountId;
	}
}
