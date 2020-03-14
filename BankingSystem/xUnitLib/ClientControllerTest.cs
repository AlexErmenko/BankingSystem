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
			//var mock = new Mock<IAsyncRepository<Client>>();
			//mock.Setup(expression: repository => repository.GetAll()).ReturnsAsync(valueFunction: Inid);

			//var controller = new ClientsController(repository: mock.Object);

			//IActionResult result =  await controller.Index();

			//ViewResult viewResult = await Assert.IsType<Task<IActionResult>>(@object: result) as ViewResult;

			//var model = Assert.IsAssignableFrom<IEnumerable<Client>>(@object: viewResult.ViewData.Model);

			//Assert.Equal(expected: 2, actual: model.Count());
		}

		public Client GetUser()
		{
			return new Client() {Id = 3, Login = "login", TelNumber = "0970716227"};
		}

		public void Delete_ClientTest()
		{
			//Arrange
			// int clientId = 3;
			// var mock = new Mock<IAsyncRepository<Client>>();
			// mock.Setup(repository => repository.GetById(clientId)).ReturnsAsync(GetUser);

			// var controller = new ClientsController(mock.Object);

			//TODO: ACT and Assert
			// await controller.Delete(clientId);




		}
	}
}
