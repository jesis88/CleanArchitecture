using API.Controllers;
using Application.Behavior;
using Application.EntityUser.Commands;
using Application.EntityUser.Commands.CommandHandlers;
using Application.Interfaces;
using Domain.Entity;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CQRS_xUnitTesting;

public class CreateUserUT
{
    private readonly Mock<IUserManagerWrapper> _userManagerMock;
    private readonly Mock<IRoleManagerWrapper> _roleManagerMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CreateUserHandler _createUserHandler;
    private readonly UserController _controller;

    public CreateUserUT()
    {
        _userManagerMock = new Mock<IUserManagerWrapper>();
        _roleManagerMock = new Mock<IRoleManagerWrapper>();
        _mediatorMock = new Mock<IMediator>();
        _controller = new UserController(_mediatorMock.Object);
        _createUserHandler = new CreateUserHandler(_userManagerMock.Object, _roleManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenCommandIsValid()
    {
        //Arrange
        var command = new CreateUserCommand
        {
            UserName = "TestUser",
            Email = "test@gmail.com",
            Password = "TestPassword123",
            ConfirmPassword = "TestPassword123",
            Role = Application.EntityUser.Commands.Role.Customer
        };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        _userManagerMock.Setup(x => x.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

        //Act
        var result = await _createUserHandler.Handle(command, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Handle_ShouldNotCreateUser_WhenInvalidCommand()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            UserName = "", // empty username
            Email = "test@gmail.com",
            Password = "TestPassword123",
            ConfirmPassword = "TestPassword123",
            Role = Application.EntityUser.Commands.Role.Customer
        };

        var validationResult = new UserRegisterValidator(_userManagerMock.Object).Validate(command);
        
        Assert.True(validationResult.Errors.Count != 0);
    }

    [Fact]
    public async Task Controller_ShouldReturnOk_WhenValidCommand()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            UserName = "TestUser",
            Email = "test@gmail.com",
            Password = "TestPassword123",
            ConfirmPassword = "TestPassword123",
            Role = Application.EntityUser.Commands.Role.Customer
        };
        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync("User created successfully");

        //Act
        var result = await _controller.GetUserRegistrationDetailsAsync(command, CancellationToken.None);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User created successfully", okResult.Value);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task Controller_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        //Arrange
        var command = new CreateUserCommand();
        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        //Act
        var result = await _controller.GetUserRegistrationDetailsAsync(command, CancellationToken.None);

        //Assert
        var resultStatusCode = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, resultStatusCode.StatusCode);
    }
}