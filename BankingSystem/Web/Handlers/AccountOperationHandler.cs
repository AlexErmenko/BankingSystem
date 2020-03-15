using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using MediatR;
using Web.Commands
	;

namespace Web.Handlers
{

	/// <summary>
	/// Обработчик для запроса по операциям по акк
	/// </summary>
	public class AccountOperationHandler : IRequestHandler<GetAccountOperationQuery,List<AccountOperationViewModel>>
	{
		private IAsyncRepository<BankAccount> AccountRepository   { get; }
		private IAsyncRepository<Operation>   OperationRepository { get; }

		public AccountOperationHandler(IAsyncRepository<BankAccount> accountRepository,
									   IAsyncRepository<Operation>   operationRepository)
		{
			AccountRepository   = accountRepository;
			OperationRepository = operationRepository;
		}

		public async Task<List<AccountOperationViewModel>> Handle(GetAccountOperationQuery request,
																  CancellationToken        cancellationToken)
		{
			var account    = await AccountRepository.GetById(request.Id);
			var operations = await OperationRepository.GetAll();

			operations.Where(operation => operation.IdAccount.Equals(account.Id));
			var viewModels = operations.Select(operation =>
			{
				var model = new AccountOperationViewModel();
				model.Amount        = operation.Amount;
				model.Type          = operation.TypeOperation;
				model.OperationTime = operation.OperationTime;
				return model;
			}).ToList();
			return viewModels;
		}
	}
}
