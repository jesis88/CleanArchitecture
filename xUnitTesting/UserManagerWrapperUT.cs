using Application.EntityUser.Query;
using Application.Interfaces;
using AutoMapper;
using Domain.Entity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTesting
{
    public class UserManagerWrapperUT
    {
        private readonly Mock<IUserStore<ApplicationUser>> _userStoreMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IMapper> _mockMapper;
        private UserManagerWrapper _userManagerWrapper;

        public UserManagerWrapperUT()
        {
            _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(_userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            _mockMapper = new Mock<IMapper>();
            _userManagerWrapper = new UserManagerWrapper(_userManagerMock.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddToRolesAsync_ShouldSucceed_WhenUserExists()
        {
            //arrange
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "AdminTest123" };
            var roles = new List<string> { Role.Admin.ToString() };

            _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName)).ReturnsAsync(new ApplicationUser { Id = user.Id, UserName = user.UserName });
            _userManagerMock.Setup(x => x.AddToRolesAsync(It.IsAny<ApplicationUser>(), roles)).ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await _userManagerWrapper.AddToRolesAsync(user, roles);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task AddToRolesAsync_ShouldThrowUserNameIsNull_WhenUserDoesNotExists()
        {
            //arrange
            var user = new User { Id = Guid.NewGuid().ToString() };
            var roles = new List<string> { Role.Admin.ToString() };

            _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName!)).ReturnsAsync(new ApplicationUser { Id = user.Id, UserName = user.UserName });

            //Act
            var error = await Assert.ThrowsAsync<InvalidOperationException>(() => _userManagerWrapper.AddToRolesAsync(user, roles));

            //Assert
            Assert.Equal("Username is null", error.Message);
        }

        [Fact]
        public async Task GetRolesAsync_ShouldReturn_CustomerRole()
        {
            //Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "TestUser" };
            var roles = new List<string> { Role.Customer.ToString() };
            _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName)).ReturnsAsync(new ApplicationUser { Id = user.Id, UserName = user.UserName });
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(roles);

            //Act
            var result = await _userManagerWrapper.GetRolesAsync(user);

            Assert.NotNull(result);
            Assert.Equal(roles[0], result[0]);
        }

        [Fact]
        public async Task CreateAsync_ShouldSucceed_WhenExistingUserCredentialsSent()
        {
            //Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "RegisteredUser123" };
            var password = "UserRegisteredPassword123#";

            _mockMapper.Setup(x => x.Map<ApplicationUser>(user)).Returns(new ApplicationUser { UserName = user.UserName});
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), password)).ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await _userManagerWrapper.CreateAsync(user, password);


            //Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task FindByNameAsync_ShouldReturnUser_WhenUserExistsWithUserName()
        {
            //Arrange
            var userName = "TestUser111";

            _userManagerMock.Setup(x => x.FindByNameAsync(userName)).ReturnsAsync(new ApplicationUser { UserName = userName});
            _mockMapper.Setup(x => x.Map<User>(It.IsAny<ApplicationUser>())).Returns(new User { Id = Guid.NewGuid().ToString(), UserName = userName});

            //Act
            var result = await _userManagerWrapper.FindByNameAsync(userName);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(userName, result.UserName);
        }

        [Fact]
        public async Task FindByNameAsync_ShouldThrowApplicationUserIsNull_WhenNoUserFound()
        {
            //Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);

            //Act
            var error = await Assert.ThrowsAsync<InvalidOperationException>(() => _userManagerWrapper.FindByNameAsync("testuser123"));

            //Assert
            Assert.NotNull(error);
            Assert.Equal("ApplicationUser is null", error.Message);
        }

        [Fact]
        public async Task FindByEmailAsync_ShouldReturnUser_WhenUserExists()
        {
            //Arrange
            var email = "testemail@gmail.com";

            _mockMapper.Setup(x => x.Map<User>(It.IsAny<ApplicationUser>())).Returns(new User { Id = Guid.NewGuid().ToString(), Email = email});
            _userManagerMock.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(new ApplicationUser { Email = email});

            //Act
            var result = await _userManagerWrapper.FindByEmailAsync(email);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task FindByEmailAsync_ShouldThrowApplicationUserIsNull_WhenUserDoesNotExists()
        {
            //Arrange
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);

            //Act
            var error = await Assert.ThrowsAsync<InvalidOperationException>(() => _userManagerWrapper.FindByEmailAsync("testemail@gmail.com"));

            //Assert
            Assert.NotNull(error);
            Assert.Equal("ApplicationUser is null", error.Message);
        }

        [Fact]
        public async Task CheckPasswordAsync_ShouldReturnTrue_WhenCredentialsMatch()
        {
            //Arrage
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "TestUser111" };
            var password = "TestUser123";

            _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName)).ReturnsAsync(new ApplicationUser { Id = user.Id, UserName = user.UserName});
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), password)).ReturnsAsync(true);

            //Act
            var result = await _userManagerWrapper.CheckPasswordAsync(user, password);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldSucceed_WhenUserFound()
        {
            //Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "testuser111", RecentLogin = DateTime.UtcNow };
            _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName)).ReturnsAsync(new ApplicationUser { UserName = user.UserName, RecentLogin = user.RecentLogin});
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await _userManagerWrapper.UpdateAsync(user);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task ToListAsync_ShouldReturnListOfUser_WhenQueryIsValid()
        {
            var options = new DbContextOptionsBuilder<CqrsDbContext>()
                .UseInMemoryDatabase(databaseName: "testDb_" + Guid.NewGuid().ToString())
                .Options;
            var userStore = new UserStore<ApplicationUser>(new CqrsDbContext(options));
            var userManager = new UserManager<ApplicationUser>(userStore, null!, null!, null!, null!, null!, null!, null!, null!);
            _userManagerWrapper = new UserManagerWrapper(userManager, _mockMapper.Object);

            var query = new GetUserListQuery() { PageNumber = 2, PageSize = 2 };
            var applicationUsers = new List<ApplicationUser>
            {
                new() {Id = Guid.NewGuid().ToString(),UserName = "testuser1"},
                new() {Id = Guid.NewGuid().ToString(),UserName = "testuser2"},
                new() {Id = Guid.NewGuid().ToString(),UserName = "testuser3"},
                new() {Id = Guid.NewGuid().ToString(), UserName = "testuser4"},
                new() {Id = Guid.NewGuid().ToString(), UserName = "testuser5"}
            };

            using (var context = new CqrsDbContext(options))
            {
                context.Users.AddRange(applicationUsers);
                await context.SaveChangesAsync();
            }

            _mockMapper.Setup(x => x.Map<List<User>>(It.IsAny<List<ApplicationUser>>())).Returns((List<ApplicationUser> source) => source.Select(a => new User { Id = a.Id, UserName = a.UserName }).ToList());

            //Act
            var result = await _userManagerWrapper.ToListAsync(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(query.PageSize, result.Count);
            Assert.Equal(applicationUsers.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).Select(a => a.UserName), result.Select(u => u.UserName));
        }
    }
}
