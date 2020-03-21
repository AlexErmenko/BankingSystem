using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using MediatR;

namespace Web.Commands
{
	public class GetClientCreditQuery : IRequest<Credit>
	{
		public GetClientCreditQuery(int? IdAccount, bool Status)
		{
			this.IdAccount = IdAccount;
			this.Status      = Status;
		}
		public int?     IdAccount { get; }
		public bool  Status      { get; }

	}
}
