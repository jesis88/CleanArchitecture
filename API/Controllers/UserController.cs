using Application.EntityUser.Commands;
using Application.EntityUser.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

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

        [AllowAnonymous]
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

        /*[Authorize(Roles = "Admin")]*/
        [HttpGet("UserList")]
        public async Task<IActionResult> GetUserListAsync([FromQuery]GetUserListQuery query, CancellationToken cancellationToken)
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

    }
}
