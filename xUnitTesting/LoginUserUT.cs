using API.Controllers;
using Application.EntityUser.Commands;
using Application.EntityUser.Commands.CommandHandlers;
using Application.Interfaces;
using Domain.Entity;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace xUnitTesting
{
    public class LoginUserUT
    {
        private readonly Mock<IUserManagerWrapper> _userManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;
        private readonly Mock<IBackgroundJobClient> _backgroundJobClientMock;
        private readonly LoginHandler _loginHandler;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _controller;

        public LoginUserUT()
        {
            _mediatorMock = new Mock<IMediator>();
            _userManagerMock = new Mock<IUserManagerWrapper>();
            _configurationMock = new Mock<IConfiguration>();
            _refreshTokenServiceMock = new Mock<IRefreshTokenService>();
            _backgroundJobClientMock = new Mock<IBackgroundJobClient>();
            _loginHandler = new LoginHandler(_userManagerMock.Object, _configurationMock.Object, _refreshTokenServiceMock.Object, _backgroundJobClientMock.Object);
            _controller = new UserController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ThrowsException()
        {
            // Arrange
            var command = new LoginCommand { Username = "test", Password = "password" };
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User)null!);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _loginHandler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_WhenPasswordIsIncorrect_ThrowsException()
        {
            // Arrange
            var command = new LoginCommand { Username = "test", Password = "password" };
            var user = new User
            {
                Id = Guid.NewGuid().ToString()
            };
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var error = await Assert.ThrowsAsync<InvalidOperationException>(() => _loginHandler.Handle(command, default));

            //Assert
            Assert.Equal("Incorrect details for login", error.Message);
        }

        [Fact]
        public async Task Handle_WhenCredentialsAreValid_ReturnsTokenData()
        {
            // Arrange
            var command = new LoginCommand { Username = "test", Password = "password" };
            var user = new User
            {
                Id = Guid.NewGuid().ToString()
            };
            var roles = new List<string> { "Admin", "Customer" };
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            _configurationMock.Setup(x => x["Jwt:Key"]).Returns("R<7(%~bhFarESf#QmA;P+UM*.B6jpcvV8?3C/Hw$g^5=K");
            _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("https://localhost:7019");
            _refreshTokenServiceMock.Setup(x => x.AddRefreshToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()));
            _refreshTokenServiceMock.Setup(x => x.GetJWTAndRefreshToken(It.IsAny<IUserManagerWrapper>(), It.IsAny<IConfiguration>(), It.IsAny<string>(), It.IsAny<User>())).ReturnsAsync(JsonConvert.SerializeObject(new { AccessToken = "TestJwtToken", RefreshToken = "TestRefreshToken" }));

            // Act
            var result = await _loginHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var tokenData = JsonConvert.DeserializeObject<dynamic>(result);
            Assert.NotNull(tokenData!.AccessToken);
            Assert.NotNull(tokenData.RefreshToken);
        }

        [Fact]
        public async Task Handle_ShouldUpdateRecentLogin_WhenLastLoginIsMoreThanOneMinuteAgo()
        {
            // Arrange
            var command = new LoginCommand
            {
                Username = "TestUser",
                Password = "TestPassword123"
            };

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = command.Username,
                RecentLogin = DateTime.UtcNow.AddMinutes(-2) // Set RecentLogin to a time more than a minute ago
            };
            var roles = new List<string> { "Admin", "Customer" };
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            _configurationMock.Setup(x => x["Jwt:Key"]).Returns("R<7(%~bhFarESf#QmA;P+UM*.B6jpcvV8?3C/Hw$g^5=K");
            _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("https://localhost:7019");
            _refreshTokenServiceMock.Setup(x => x.AddRefreshToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()));

            _userManagerMock.Setup(x => x.FindByNameAsync(command.Username))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password))
                .ReturnsAsync(true);
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .Callback<User>(u => Assert.True(u.RecentLogin >= DateTime.UtcNow.AddSeconds(-5))) // Allow for up to 5 seconds of test execution time
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _loginHandler.Handle(command, CancellationToken.None);

            // Assert
            // Assert that UpdateAsync was called (this will throw if the callback assertion fails)
            _userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Controller_ShouldReturnOk_WhenCommandIsValid()
        {
            // Arrange
            var command = new LoginCommand
            {
                Username = "TestUser",
                Password = "TestPassword123"
            };

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(JsonConvert.SerializeObject(new { AccessToken = "testToken", RefreshToken = "testRefreshToken" }));

            // Act
            var result = await _controller.GetUserLoginDetailsAsync(command, CancellationToken.None);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenData = JsonConvert.DeserializeObject<JObject>(okResult.Value!.ToString()!);
            Assert.Equal("testToken", tokenData!["AccessToken"]!.ToString());
            Assert.Equal("testRefreshToken", tokenData["RefreshToken"]!.ToString());
        }

        [Fact]
        public async Task Controller_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var command = new LoginCommand
            {
                Username = "TestUser",
                Password = "TestPassword123"
            };

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetUserLoginDetailsAsync(command, CancellationToken.None);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

    }
}
