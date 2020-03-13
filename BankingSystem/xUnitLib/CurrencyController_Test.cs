using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;

using Web.Controllers;
using Web.Services;
using Web.ViewModels;

using Xunit;

namespace xUnitLib
{
	public class CurrencyControllerTest
	{
		private List<CurrencyViewModel> GetTestSessions()

		{
			var sessions = new List<CurrencyViewModel>();

			sessions.Add(item: new CurrencyViewModel

			{
				Id = 1,
				Name = "USD",
				BuyRate = 24.04m,
				SaleRate = 23.52m
			});

			sessions.Add(item: new CurrencyViewModel

			{
				Id = 3,
				Name = "EUR",
				BuyRate = 27.04m,
				SaleRate = 29.52m
			});

			return sessions;
		}

		public List<ClientAccountViewModel> GetClientAccountViewModelData()
		{
			var list = new List<ClientAccountViewModel>();

			list.Add(new ClientAccountViewModel()
			{
				Currency = "USD",
				Amount = 20.4m,
				AccountType = "",
				DateClose = DateTime.Now,
				DateOpen = DateTime.Now

			});


			list.Add(new ClientAccountViewModel()
			{
				Currency    = "EUR",
				Amount      = 19.5m,
				AccountType = "",
				DateClose   = DateTime.Now,
				DateOpen    = DateTime.Now

			});

			return list;
		}

		[Fact]
		public async void GetInfo_Return_ClientViewModel()
		{
			//Arrange
			var mock = new Mock<ICurrencyViewModelService>();

			mock.Setup(service => service.GetCurrencyRate()).ReturnsAsync(GetTestSessions());

			// var controller = new CurrencyController(currencyViewModelSerivce: mock.Object);

			//Act
			// var info = await controller.GetInfo();

			//Assert
			// var viewResult = Assert.IsType<ViewResult>(@object: info);
			//
			// var model = Assert.IsAssignableFrom<IEnumerable<CurrencyViewModel>>(@object: viewResult.ViewData.Model);
			//
			// Assert.Equal(expected: 2, actual: model.Count());
		}


		[Fact]
		public async void Index_ReturnClientAccountViewModel()
		{
			//Arrange
			// var mock = new Mock<ICurrencyViewModelService>();
			// mock.Setup(service => service.GetClientAccounts(3)).ReturnsAsync(GetClientAccountViewModelData());
			// var controller = new CurrencyController(mock.Object);

			//Act
			// var index = await controller.Index();

			//Assert
			// var viewResult = Assert.IsType<ViewResult>(index);
			// var models = Assert.IsAssignableFrom<IEnumerable<ClientAccountViewModel>>(viewResult.ViewData.Model);
			// Assert.Equal(expected: 2, actual: models.Count());
		}
	}
}
