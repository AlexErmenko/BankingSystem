using System.Collections.Generic;
using MediatR;

namespace Web.Commands
{
	/// <summary>
	/// Запрос на получение всех операций по аккаунту
	/// </summary>
	public class GetAccountOperationQuery : IRequest<List<AccountOperationViewModel>>
	{
		public int Id { get; set; }

		public GetAccountOperationQuery(int id) { Id = id; }
	}
}
