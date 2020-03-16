using System;
using System.Threading;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using MediatR;

namespace Web.Handlers
{
	public class ClientAccountOperationHandler : AsyncRequestHandler<ClientAccountOperationCommand>
	{
		public ClientAccountOperationHandler(IAsyncRepository<Operation> Repository) { this.Repository = Repository; }
		private IAsyncRepository<Operation> Repository { get; set; }

		protected override async Task Handle(ClientAccountOperationCommand request, CancellationToken cancellationToken)
		{
			var entity = new Operation();
			entity.IdAccount = request.IdAccount;
			entity.OperationTime = DateTime.Now;
			entity.TypeOperation = request.Type;
			entity.Amount = request.Amount;
			await Repository.AddAsync(entity);
		}
	}
}
