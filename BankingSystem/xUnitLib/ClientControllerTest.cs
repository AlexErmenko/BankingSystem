using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Web.Controllers;

using Xunit;

namespace xUnitLib
{
	public class ClientControllerTest
	{
		public List<Client> Inid()
		{
			var clients = new List<Client>();
			clients.Add(item: new Client
			{
				Id = 1,
				Login = "11"
			});
			clients.Add(item: new Client
			{
				Id = 2,
				Login = "22"
			});
			return clients;
		}

		[Fact]
		public async void Index_test()
		{
			var mock = new Mock<IAsyncRepository<Client>>();
			mock.Setup(expression: repository => repository.GetAll()).ReturnsAsync(valueFunction: Inid);

			var controller = new ClientsController(repository: mock.Object);

			Task<IActionResult> result =  controller.Index();

			ViewResult viewResult = await Assert.IsType<Task<IActionResult>>(@object: result) as ViewResult;

			var model = Assert.IsAssignableFrom<IEnumerable<Client>>(@object: viewResult.ViewData.Model);

			Assert.Equal(expected: 2, actual: model.Count());
		}
	}
}
