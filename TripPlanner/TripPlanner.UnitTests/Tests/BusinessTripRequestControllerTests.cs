using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.Controllers.Controllers;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Abstractions;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.UnitTests.Tests
{
    [TestFixture]
    public class BusinessTripRequestControllerTests
    {
        private Mock<IBusinessTripRequestRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private BusinessTripRequestController _controller;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IBusinessTripRequestRepository>();
            _mapperMock = new Mock<IMapper>();
            _controller = new BusinessTripRequestController(_repositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Post_ValidData_ReturnsOk()
        {
            // Arrange
            var apiModel = new RegisterBusinessTripApiModel { };
            var dtoModel = new RegisterBusinessTripDto { };
            var businessTripRequest = new BusinessTripRequest { };

            _mapperMock.Setup(m => m.Map<RegisterBusinessTripDto>(apiModel)).Returns(dtoModel);
            _mapperMock.Setup(m => m.Map<BusinessTripRequest>(dtoModel)).Returns(businessTripRequest);
            _repositoryMock.Setup(repo => repo.CreateTrip(businessTripRequest)).ReturnsAsync(true);

            // Act
            var result = await _controller.Post(apiModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(true, okResult.Value);
        }

        [Test]
        public async Task Post_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var apiModel = new RegisterBusinessTripApiModel { };
            _controller.ModelState.AddModelError("error", "Sample error");

            // Act
            var result = await _controller.Post(apiModel);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task Post_RepositoryException_ReturnsInternalServerError()
        {
            // Arrange
            var apiModel = new RegisterBusinessTripApiModel { };
            var dtoModel = new RegisterBusinessTripDto { };
            var businessTripRequest = new BusinessTripRequest { };

            _mapperMock.Setup(m => m.Map<RegisterBusinessTripDto>(apiModel)).Returns(dtoModel);
            _mapperMock.Setup(m => m.Map<BusinessTripRequest>(dtoModel)).Returns(businessTripRequest);
            _repositoryMock.Setup(repo => repo.CreateTrip(businessTripRequest)).Throws(new Exception("Simulated Exception"));

            // Act
            var result = await _controller.Post(apiModel);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var statusCode = (result as ObjectResult)?.StatusCode;
            Assert.IsNotNull(statusCode);
            Assert.AreEqual(500, statusCode.Value);
            Assert.AreEqual("Internal Server Error: Simulated Exception", (result as ObjectResult)?.Value);
        }

        [Test]
        public async Task Post_MapperException_ReturnsInternalServerError()
        {
            // Arrange
            var apiModel = new RegisterBusinessTripApiModel { };
            _mapperMock.Setup(m => m.Map<RegisterBusinessTripDto>(apiModel)).Throws(new Exception("Simulated Exception"));

            // Act
            var result = await _controller.Post(apiModel);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var statusCode = (result as ObjectResult)?.StatusCode;
            Assert.IsNotNull(statusCode);
            Assert.AreEqual(500, statusCode.Value);
            Assert.AreEqual("Internal Server Error: Simulated Exception", (result as ObjectResult)?.Value);
        }

        [Test]
        public async Task Post_AuthorizedUser_ReturnsOk()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext { HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext() };
            _repositoryMock.Setup(repo => repo.CreateTrip(It.IsAny<BusinessTripRequest>())).ReturnsAsync(true);

            // Act
            var result = await _controller.Post(new RegisterBusinessTripApiModel());

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(true, okResult.Value);
        }
    }
}
