using System;

using MediatR;

using Web.ViewModels;

namespace Web.Commands
{
	/// <summary>
	///     Запрос на получение всех операций по аккаунту
	/// </summary>
	public class GetAccountOperationQuery : IRequest<AccountOperationViewModel>
	{
		public int Id { get; set; }
		public DateTime? StartPeriod { get; set; }
		public DateTime? EndPeriod { get; set; }

		public GetAccountOperationQuery(int id, DateTime? startPeriod, DateTime? endPeriod)
		{
			Id = id;
			StartPeriod = startPeriod;
			EndPeriod = endPeriod;
		}
	}
}
