using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Web.Commands;

namespace Web.Controllers
{
	public class AccountOperationByPeriodHandler : IRequestHandler<GetAccountOperationByPeriod, List<AccountOperationViewModel>>
	{
		public Task<List<AccountOperationViewModel>> Handle(GetAccountOperationByPeriod request, CancellationToken cancellationToken) => throw new NotImplementedException();
	}
}
