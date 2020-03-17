using System;

using MediatR;

namespace Web.Commands
{
	public class GetUserByIdQuery : IRequest<int?>
	{
		public string Login { get; set; }
		public GetUserByIdQuery(string Login) => this.Login = Login;
	}
}
