using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.Controllers.Controllers;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.DatabaseModels.Models.Enums;
using TripPlanner.Logic.Abstractions;
using TripPlanner.Logic.Common;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.UnitTests.Tests
{
    [TestFixture]
    public class ViewBusinessTripsControllerTests
    {
        private ViewBusinessTripsController _controller;
        private Mock<IBusinessTripRequestRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IBusinessTripRequestRepository>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ViewBusinessTripsController(_repositoryMock.Object, _mapperMock.Object);
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "testuser@yahoo.com"),
            };
            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };
        }

        [Test]
        public async Task GetTrips_ValidSearchCriteria_ReturnsTrips()
        {
            // Arrange
            var searchCriteria = new SearchCriteriaApiModel
            {
                ClientLocation = "New York",
                StartDate = DateTimeOffset.Now,
                EndDate = DateTimeOffset.Now.AddDays(7),
            };

            var searchCriteriaDto = new SearchCriteria
            (
                "New York",
                null,
                null,
                null,
                DateTimeOffset.Now,
                DateTimeOffset.Now.AddDays(7)
            );

            _mapperMock.Setup(m => m.Map<SearchCriteria>(searchCriteria)).Returns(searchCriteriaDto);
            _repositoryMock.Setup(r => r.GetTripsByCriteria(searchCriteriaDto, It.IsAny<string>()))
                .ReturnsAsync(new List<IBusinessTrip>());

            // Act
            var result = await _controller.GetTrips(searchCriteria);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<IBusinessTrip>>(result);
        }

        [Test]
        public async Task GetTrips_InvalidSearchCriteria_ReturnsNothing()
        {
            // Arrange
            var invalidSearchCriteria = new SearchCriteriaApiModel
            {
                ClientLocation = "City55",
                StartDate = DateTimeOffset.Now,
                EndDate = DateTimeOffset.Now.AddDays(-7),
            };
            _repositoryMock.Setup(r => r.GetTripsByCriteria(It.IsAny<SearchCriteria>(), It.IsAny<string>()))
                 .ReturnsAsync(new List<IBusinessTrip>());
            // Act
            var result = await _controller.GetTrips(invalidSearchCriteria);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateTripStatus_ValidInput_ReturnsTrue()
        {
            // Arrange
            var tripId = Guid.NewGuid();
            var updateStatusApiModel = new UpdateStatusApiModel
            {
                Status = RequestStatus.Accepted,
            };

            var updateStatusModel = new UpdateStatusModel
            {
                Status = RequestStatus.Accepted,
            };

            _mapperMock.Setup(m => m.Map<UpdateStatusModel>(updateStatusApiModel)).Returns(updateStatusModel);
            _repositoryMock.Setup(r => r.UpdateStatus(tripId, updateStatusModel, It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateTripStatus(tripId, updateStatusApiModel);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateTripStatus_RepositoryFails_ReturnsFalse()
        {
            // Arrange
            var tripId = Guid.NewGuid();
            var updateStatusApiModel = new UpdateStatusApiModel
            {
                Status = RequestStatus.Accepted,
            };

            var updateStatusModel = new UpdateStatusModel
            {
                Status = RequestStatus.Accepted,
            };

            _mapperMock.Setup(m => m.Map<UpdateStatusModel>(updateStatusApiModel)).Returns(updateStatusModel);
            _repositoryMock.Setup(r => r.UpdateStatus(tripId, updateStatusModel, It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateTripStatus(tripId, updateStatusApiModel);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetTrips_ValidSearchCriteria_EmptyResult_ReturnsEmptyList()
        {
            // Arrange
            var searchCriteria = new SearchCriteriaApiModel
            {
                Status = RequestStatus.Accepted,
            };

            var searchCriteriaDto = new SearchCriteria(null, null, null, RequestStatus.Accepted, null, null);

            _mapperMock.Setup(m => m.Map<SearchCriteria>(searchCriteria)).Returns(searchCriteriaDto);
            _repositoryMock.Setup(r => r.GetTripsByCriteria(searchCriteriaDto, It.IsAny<string>()))
                .ReturnsAsync(new List<IBusinessTrip>());

            // Act
            var result = await _controller.GetTrips(searchCriteria);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }
    }
}
