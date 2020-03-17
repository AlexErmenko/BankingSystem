using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Web.Commands.Web.Commands;

namespace Web.Handlers
{
	/// <summary>
	/// Проверка на наличие кредита у пользователя
	/// </summary>
	public class CheckByCreditHandler : IRequestHandler<GetClientCreditQuery,Credit>
	{
		private readonly IAsyncRepository<Credit> Repository;
		public CheckByCreditHandler(IAsyncRepository<Credit> Repository) => this.Repository = Repository;

		public async Task<Credit> Handle(GetClientCreditQuery request, CancellationToken cancellationToken)
		{
			var validator = new PasswordValidator<ApplicationUser>();
			//var result    = await validator.ValidateAsync(manager: UserManager, user: request.User, password: request.Password);
			var result = await Repository.GetById(request.IdAccount);
			return result;
		}
	}
}
