﻿using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Moq;

using Web.Controllers;

using Xunit;

namespace xUnitLib
{
  public class HomeControllerTest
  {
    [Fact]
    public async Task Index_ReturnsAViewResult_WithAListOfBrainstormSessions()

    {
      // Arrange

      var mockRepo = new Mock<ILogger<HomeController>>();

      // var controller = new HomeController(logger: mockRepo.Object);

      // Act

      // var result = await controller.Index();

      // Assert

      // var viewResult = Assert.IsType<ViewResult>(@object: result);

      /*var model = Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(

                viewResult.ViewData.Model);

            Assert.Equal(2, model.Count());*/
    }

    /*#region snippet_ModelState_ValidOrInvalid

        [Fact]

        public async Task IndexPost_ReturnsBadRequestResult_WhenModelStateIsInvalid()

        {

            // Arrange

            var mockRepo = new Mock<IBrainstormSessionRepository>();

            mockRepo.Setup(repo => repo.ListAsync())

                .ReturnsAsync(GetTestSessions());

            var controller = new HomeController(mockRepo.Object);

            controller.ModelState.AddModelError("SessionName", "Required");

            var newSession = new HomeController.NewSessionModel();



            // Act

            var result = await controller.Index(newSession);



            // Assert

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.IsType<SerializableError>(badRequestResult.Value);

        }



        [Fact]

        public async Task IndexPost_ReturnsARedirectAndAddsSession_WhenModelStateIsValid()

        {

            // Arrange

            var mockRepo = new Mock<IBrainstormSessionRepository>();

            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<BrainstormSession>()))

                .Returns(Task.CompletedTask)

                .Verifiable();

            var controller = new HomeController(mockRepo.Object);

            var newSession = new HomeController.NewSessionModel()

            {

                SessionName = "Test Name"

            };



            // Act

            var result = await controller.Index(newSession);



            // Assert

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Null(redirectToActionResult.ControllerName);

            Assert.Equal("Index", redirectToActionResult.ActionName);

            mockRepo.Verify();

        }

        #endregion*/

    /*#region snippet_GetTestSessions

        private List<BrainstormSession> GetTestSessions()

        {

            var sessions = new List<BrainstormSession>();

            sessions.Add(new BrainstormSession()

            {

                DateCreated = new DateTime(2016, 7, 2),

                Id = 1,

                Name = "Test One"

            });

            sessions.Add(new BrainstormSession()

            {

                DateCreated = new DateTime(2016, 7, 1),

                Id = 2,

                Name = "Test Two"

            });

            return sessions;

        }

        #endregion*/
  }
}
