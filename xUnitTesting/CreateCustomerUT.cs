using API.Controllers;
using Application.Behavior;
using Application.EntityCustomer.Commands;
using Application.EntityCustomer.Commands.CommandHandlers;
using Application.Interfaces;
using Domain.Entity;
using FluentValidation;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CQRS_xUnitTesting
{
    public class CreateCustomerUT
    {
        private readonly CreateCustomerHandler _handler;
        private readonly Mock<IMediator> _mockMediator;
        private readonly CustomerController _controller;
        private readonly Mock<ICustomerWrapper> _mockCustomer;
        private readonly Mock<IUserManagerWrapper> _mockUserManager;

        public CreateCustomerUT()
        {
            _mockMediator = new Mock<IMediator>();
            _mockCustomer = new Mock<ICustomerWrapper>();
            _mockUserManager = new Mock<IUserManagerWrapper>();
            _handler = new CreateCustomerHandler(_mockCustomer.Object, _mockUserManager.Object);
            _controller = new CustomerController(_mockMediator.Object);
        }

        [Theory]
        [MemberData(nameof(CreateCustomerCommandTest.ValidData),MemberType = typeof(CreateCustomerCommandTest))]
        public async Task Handle_ShouldCreateCustomer_WhenCommandIsValid(string name, string address, string phNumber, bool status, string userName)
        {
            //Arrage
            var command = new CreateCustomerCommand
            {
                Name = name,
                UserName = userName,
                Address = address,
                PhoneNumber = phNumber,
                Active = status
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync(new User 
                { 
                    Id = Guid.NewGuid().ToString(), 
                    UserName = command.UserName,
                    Email = "jes88@gmail.com",
                    Role = Role.Customer,
                });
            _mockCustomer.Setup(x => x.AddCustomer(It.IsAny<Customer>())).ReturnsAsync(1);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(1, result);
        }

        [Theory]
        [MemberData(nameof(CreateCustomerCommandTest.InvalidData), MemberType = typeof(CreateCustomerCommandTest))]
        public void Handle_ShouldNotCreateCustomer_WhenEmptyUserName(string name, string address, string phNumber, bool status, string userName)
        {
            //Arrange
            var command = new CreateCustomerCommand
            {
                Name = name,
                UserName = userName,
                Address = address,
                PhoneNumber = phNumber,
                Active = status
            };

            //Act and Assert
            var validationResult = new CreateCustomerValidator().Validate(command);

            Assert.True(validationResult.Errors.Count != 0);
        }

        [Theory]
        [MemberData(nameof(CreateCustomerCommandTest.ValidData), MemberType = typeof(CreateCustomerCommandTest))]
        public async Task Controller_ShouldReturnResponseOk_WithValidData(string name, string address, string phNumber, bool status, string userName)
        {
            //Arrange
            var command = new CreateCustomerCommand
            {
                Name = name,
                UserName = userName,
                Address = address,
                PhoneNumber = phNumber,
                Active = status
            };

            _mockMediator.Setup( c => c.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            //Act
            var result = await _controller.CreateCustomer(command, CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Theory]
        [MemberData(nameof(CreateCustomerCommandTest.ValidData), MemberType = typeof(CreateCustomerCommandTest))]
        public async Task Controller_ShouldReturnResponseNull_AfterExceptionThrown(string name, string address, string phNumber, bool status, string userName)
        {
            //Arrange
            var command = new CreateCustomerCommand
            {
                Name = name,
                UserName = userName,
                Address = address,
                PhoneNumber = phNumber,
                Active = status
            };

            _mockMediator.Setup(c => c.Send(command, It.IsAny<CancellationToken>())); //not setting any return type

            //Act
            var result = await _controller.CreateCustomer(command, CancellationToken.None);

            //Assert
            var statusCode = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCode.StatusCode);
        }
    }

    public static class CreateCustomerCommandTest
    {
        public static IEnumerable<object[]> ValidData
        {
            get
            {
                yield return new object[]
                {
                    "TestName",
                    "TestAddress",
                    "9813131313",
                    true,
                    "TestUserName"
                };
            }
        }

        public static IEnumerable<object[]> InvalidData
        {
            get
            {
                yield return new object[]
                {
                    "testCustName",
                    "testAdd",
                    "9811111111",
                    true,
                    ""
                };
            }
        }
    }
}
