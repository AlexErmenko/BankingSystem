using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using MediatR;

using Web.Commands;
using Web.ViewModels;

namespace Web.Handlers
{
	/// <summary>
	///     Обработчик для запроса по операциям по акк
	/// </summary>
	public class AccountOperationHandler : IRequestHandler<GetAccountOperationQuery, AccountOperationViewModel>
	{
		private IAsyncRepository<BankAccount> AccountRepository { get; }
		private IAsyncRepository<Operation> OperationRepository { get; }

		public AccountOperationHandler(IAsyncRepository<BankAccount> accountRepository, IAsyncRepository<Operation> operationRepository)
		{
			AccountRepository = accountRepository;
			OperationRepository = operationRepository;
		}

		public async Task<AccountOperationViewModel> Handle(GetAccountOperationQuery request, CancellationToken cancellationToken)
		{
			//TODO:?
			var account = await AccountRepository.GetById(id: request.Id);

			//Получаем все операции
			var operations = await OperationRepository.GetAll();

			//Фильтруем их по Id аккаунта
			operations = operations.Where(predicate: operation => operation.IdAccount.Equals(obj: request.Id)).ToList();

			switch (request.StartPeriod)
			{
				case null:
					Console.WriteLine(value: request.StartPeriod);
					break;
				default:
					operations = operations.Where(predicate: it => it.OperationTime > request.StartPeriod).ToList();
					break;
			}

			switch (request.EndPeriod)
			{
				case null:
					Console.WriteLine(value: request.EndPeriod);
					break;
				default:
					operations = operations.Where(predicate: it => it.OperationTime < request.EndPeriod).ToList();
					break;
			}

			var viewModel = new AccountOperationViewModel
			{
				IdAccount = request.Id,
				Operations = operations,
				StartPeriod = request.StartPeriod,
				EndPeriod = request.EndPeriod
			};

			return viewModel;
		}
	}
}
