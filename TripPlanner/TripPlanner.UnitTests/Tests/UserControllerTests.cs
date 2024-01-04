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
    public class UserControllerTests
    {
        private UserController _userController;
        private Mock<IAuthenticationRepository> _authRepositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _authRepositoryMock = new Mock<IAuthenticationRepository>();
            _mapperMock = new Mock<IMapper>();
            _userController = new UserController(_authRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Register_ValidInput_ReturnsTrue()
        {
            // Arrange
            var registerApiModel = new RegisterApiModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "StrongPassword",
                ConfirmationPassword = "StrongPassword"
            };

            var registerUserDto = new RegisterUserDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "StrongPassword",
                ConfirmationPassword = "StrongPassword"
            };

            _mapperMock.Setup(m => m.Map<RegisterUserDto>(registerApiModel)).Returns(registerUserDto);
            _mapperMock.Setup(m => m.Map<User>(registerUserDto)).Returns(new User());

            _authRepositoryMock.Setup(r => r.Register(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _userController.Register(registerApiModel);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Register_RepositoryFails_ReturnsFalse()
        {
            // Arrange
            var registerApiModel = new RegisterApiModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "StrongPassword",
                ConfirmationPassword = "StrongPassword"
            };

            _mapperMock.Setup(m => m.Map<RegisterUserDto>(registerApiModel)).Returns(new RegisterUserDto());
            _mapperMock.Setup(m => m.Map<User>(It.IsAny<RegisterUserDto>())).Returns(new User());

            _authRepositoryMock.Setup(r => r.Register(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _userController.Register(registerApiModel);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Login_ValidInput_ReturnsTokenDto()
        {
            // Arrange
            var loginApiModel = new LoginApiModel
            {
                Email = "john.doe@example.com",
                Password = "StrongPassword"
            };

            var loginDto = new LoginDto
            {
                Email = "john.doe@example.com",
                Password = "StrongPassword"
            };

            _mapperMock.Setup(m => m.Map<LoginDto>(loginApiModel)).Returns(loginDto);
            _authRepositoryMock.Setup(r => r.Login(It.IsAny<LoginDto>())).ReturnsAsync(new TokenDto());

            // Act
            var result = await _userController.Login(loginApiModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<TokenDto>(result);
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsNullToken()
        {
            // Arrange
            var loginApiModel = new LoginApiModel
            {
                Email = "john.doe@example.com",
                Password = "IncorrectPassword"
            };

            _authRepositoryMock.Setup(r => r.Login(It.IsAny<LoginDto>())).ReturnsAsync((TokenDto)null);

            // Act
            var result = await _userController.Login(loginApiModel);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Logout_ValidUser_CallsLogoutMethod()
        {
            // Arrange

            // Act
            await _userController.Logout();

            // Assert
            _authRepositoryMock.Verify(r => r.Logout(), Times.Once);
        }
    }
}
