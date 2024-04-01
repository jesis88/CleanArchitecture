using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

        [AllowAnonymous]
        [HttpPost("Registration")]
        public async Task<IActionResult> GetUserRegistrationDetailsAsync([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        /*[AllowAnonymous]
        [HttpGet("Login")]
        public async Task<IActionResult> GetUserLoginDetailsAsync([FromQuery] LoginCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] RefreshTokenQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(query, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("UserList")]
        public async Task<IActionResult> GetUserListAsync(CancellationToken cancellationToken)
        {
            try
            {
                var query = new GetUserListQuery();
                var response = await _mediator.Send(query, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }*/

    }
}
