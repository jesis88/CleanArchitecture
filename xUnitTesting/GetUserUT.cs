using API.Controllers;
using Application.Behavior;
using Application.EntityUser.Query;
using Application.EntityUser.Query.QueryHandler;
using Application.Interfaces;
using Domain.Entity;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Text;
using System.Text.Json;

namespace CQRS_xUnitTesting
{
    public class GetUserUT
    {
        private readonly Mock<IDistributedCache> _mockDistributedCache;
        private readonly Mock<IUserManagerWrapper> _mockUserManager;
        private readonly GetUserListHandler _handler;
        private readonly UserController _controller;
        private readonly Mock<IMediator> _mediatorMock;

        public GetUserUT()
        {
            _mediatorMock = new Mock<IMediator>();
            _mockDistributedCache = new Mock<IDistributedCache>();
            _mockUserManager = new Mock<IUserManagerWrapper>();

            _handler = new GetUserListHandler(_mockUserManager.Object, _mockDistributedCache.Object);
            _controller = new UserController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnUserListFromDB_WhenCacheIsEmpty()
        {
            //Arrange
            var userList = new List<User> { new() { Id = Guid.NewGuid().ToString() } };
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            var query = new GetUserListQuery();

            //Setup Mock
            _mockDistributedCache.Setup(cache => cache.Get(It.IsAny<string>()));
            _mockUserManager.Setup(x => x.ToListAsync(It.IsAny<GetUserListQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(userList);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task Handle_ReturnUserListFromDB_WhenCacheIsNotEmpty()
        {
            //Arrange
            var userList = new List<User> { new () {Id = Guid.NewGuid().ToString() } };
            var serializedUserList = JsonSerializer.Serialize(userList);
            _mockDistributedCache.Setup(cache => cache.Get(It.IsAny<string>()))
                .Returns(Encoding.UTF8.GetBytes(serializedUserList));
            var query = new GetUserListQuery();

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.Equal(userList.Count, result.Count);
        }

        [Fact]
        public async Task Controller_ShouldReturnOk_WhenQueryIsValid()
        {
            // Arrange
            var query = new GetUserListQuery();
            var userList = new List<User> { new () {Id = Guid.NewGuid().ToString() }, new () { Id = Guid.NewGuid().ToString() } };

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetUserListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userList);

            // Act
            var result = await _controller.GetUserListAsync(query , CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUserList = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(userList.Count, returnedUserList.Count);
        }

        [Fact]
        public async Task Controller_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetUserListQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetUserListAsync(new GetUserListQuery(), CancellationToken.None);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

    }
}
