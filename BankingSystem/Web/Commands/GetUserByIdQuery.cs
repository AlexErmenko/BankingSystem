using System;

using MediatR;

namespace Web.Controllers
{
	public class GetUserByIdQuery : IRequest<int?>
	{
		public string Login { get; set; }
		public GetUserByIdQuery(string Login) => this.Login = Login;
	}
}
