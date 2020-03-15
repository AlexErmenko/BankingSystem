using System;

using MediatR;

namespace Web.Controllers
{
	public class GetUserByIdQuery : IRequest<int?>
	{
		public GetUserByIdQuery(string Login) { this.Login = Login; }
		public string Login { get; set; }
	}
}
