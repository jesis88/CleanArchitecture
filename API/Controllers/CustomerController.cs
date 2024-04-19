using Application.EntityCustomer.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [Authorize(Roles = "Admin")]
        [HttpGet("Tokens")]
        public IActionResult TestAuthorization()
        {
            return Ok("You're Authorized as Admin");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(command, cancellationToken);
                return response != 1 ? throw new InvalidOperationException("Response is null!") : Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}
