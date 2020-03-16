using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using MediatR;

namespace Web.Controllers
{
	public class UserByIdHandler : IRequestHandler<GetUserByIdQuery, int?>
	{
		private IAsyncRepository<Client> ClientRepository { get; }
		public UserByIdHandler(IAsyncRepository<Client> ClientRepository) => this.ClientRepository = ClientRepository;

		public async Task<int?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{
			var clients = await ClientRepository.GetAll();
			var client = clients.FirstOrDefault(predicate: c => c.Login.Equals(value: request.Login));

			return client?.Id;
		}
	}
}
