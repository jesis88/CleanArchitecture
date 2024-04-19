using API.Controllers;
using Application.EntityUser.Query;
using Application.EntityUser.Query.QueryHandler;
using Application.Interfaces;
using Domain.Entity;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace xUnitTesting
{
    public class RefreshTokenUT
    {
        private readonly Mock<IUserManagerWrapper> _userManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;
        private readonly RefreshTokenHandler _handler;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _controller;

        public RefreshTokenUT()
        {
            _userManagerMock = new Mock<IUserManagerWrapper>();
            _configurationMock = new Mock<IConfiguration>();
            _refreshTokenServiceMock = new Mock<IRefreshTokenService>();
            _handler = new RefreshTokenHandler(_userManagerMock.Object, _configurationMock.Object, _refreshTokenServiceMock.Object);
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserController(_mediatorMock.Object);

            var roles = new List<string> { "Admin", "Customer" };
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User() { Id = Guid.NewGuid().ToString(), UserName = "username" });
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(roles);
            _configurationMock.Setup(x => x["Jwt:Key"])
                .Returns("R<7(%~bhFarESf#QmA;P+UM*.B6jpcvV8?3C/Hw$g^5=KR<7(%~bhFarESf#QmA;P+UM*.B6jpcvV8?3C/Hw$g^5=K");
            _configurationMock.Setup(x => x["Jwt:Issuer"])
                .Returns("https://localhost:7019");
            _refreshTokenServiceMock.Setup(x => x.AddRefreshToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()));
            _refreshTokenServiceMock.Setup(x => x.GetJWTAndRefreshToken(It.IsAny<IUserManagerWrapper>(), It.IsAny<IConfiguration>(), It.IsAny<string>(), It.IsAny<User>())).ReturnsAsync(JsonConvert.SerializeObject(new { AccessToken = "TestJwtToken", RefreshToken = "TestRefreshToken" }));
        }

        [Fact]
        public async Task Handle_WhenValidRefreshToken_ReturnJwtToken()
        {
            //Arrange
            var refreshTokenQuery = new RefreshTokenQuery { RefreshToken = "TestRefreshToken" };
            _refreshTokenServiceMock.Setup(x => x.GetRefreshToken(It.IsAny<string>())).Returns((UserName: "username", ExpiryDate: DateTime.Now.AddMinutes(5)));
            //Act
            var result = await _handler.Handle(refreshTokenQuery, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            var tokenData = JsonConvert.DeserializeObject<dynamic>(result);
            Assert.NotNull(tokenData!.AccessToken);
            Assert.NotNull(tokenData.RefreshToken);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenNullRefreshToken()
        {
            //Arrange
            var query = new RefreshTokenQuery { RefreshToken = "invalid_refresh_token" };
            _refreshTokenServiceMock.Setup(x => x.GetRefreshToken("invalid_refresh_token")).Returns(value: null);

            //Act and Assert
            var error = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(query, default));
            Assert.Equal("Invalid Refresh Token", error.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenExpiredRefreshToken()
        {
            //Arrange
            var query = new RefreshTokenQuery { RefreshToken = "expired_refresh_token" };
            _refreshTokenServiceMock.Setup(x => x.GetRefreshToken(It.IsAny<string>()))
                .Returns((UserName: "username", ExpiryDate: DateTime.Now.AddMinutes(-1)));

            //Act and Assert
            var error = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(query, default));
            Assert.Equal("Refresh Token has expired", error.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenInvalidUser()
        {
            //Arrange
            var query = new RefreshTokenQuery { RefreshToken = "valid_refresh_token" };
            _refreshTokenServiceMock.Setup(x => x.GetRefreshToken(It.IsAny<string>()))
                .Returns((UserName: "invalid_username", ExpiryDate: DateTime.Now.AddMinutes(5)));
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null!);

            //Act and Assert
            var error = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(query, default));
            Assert.Equal("Invalid User", error.Message);
        }

        [Fact]
        public async Task Controller_ShouldReturnOk_WhenRefreshTokenValid()
        {
            // Arrange
            var query = new RefreshTokenQuery
            {
                RefreshToken = "ValidRefreshToken"
            };

            _mediatorMock.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                .ReturnsAsync(JsonConvert.SerializeObject(new { AccessToken = "testToken", RefreshToken = "testRefreshToken" }));

            // Act
            var result = await _controller.RefreshToken(query, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenData = JsonConvert.DeserializeObject<JObject>(okResult.Value!.ToString()!);
            Assert.Equal("testToken", tokenData!["AccessToken"]!.ToString());
            Assert.Equal("testRefreshToken", tokenData["RefreshToken"]!.ToString());
        }

        [Fact]
        public async Task Controller_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var query = new RefreshTokenQuery
            {
                RefreshToken = "InvalidRefreshToken"
            };

            _mediatorMock.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.RefreshToken(query, CancellationToken.None);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void GetRefreshToken_ReturnCorrectToken()
        {
            //Arrange
            var refreshToken = "testToken";
            var expectedUsername = "testUser";
            var expectedExpiryDate = DateTime.Now.AddDays(1);
            var refreshTokenService = new RefreshTokenService();
            refreshTokenService.AddRefreshToken(refreshToken, expectedUsername, expectedExpiryDate);

            //Act
            var result = refreshTokenService.GetRefreshToken(refreshToken);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUsername, result.Value.UserName);
            Assert.Equal(expectedExpiryDate, result.Value.ExpiryDate);
        }

        [Fact]
        public void GetRefreshToken_ReturnNull_WhenTokenDoesNotExist()
        {
            //Arrange
            var refreshTokenService = new RefreshTokenService();

            //Act
            var result = refreshTokenService.GetRefreshToken("nonexistingToken");

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetUserName_ReturnsCorrectUserName()
        {
            //Arrange
            var expectedUsername = "testUser";
            var refreshTokenService = new RefreshTokenService();
            refreshTokenService.AddRefreshToken("token1", expectedUsername, DateTime.Now.AddDays(1));

            //Act
            var result = refreshTokenService.GetUserName();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUsername, result);
        }
    }
}
