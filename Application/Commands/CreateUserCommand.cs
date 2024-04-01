using MediatR;

namespace Application.Commands
{
    public enum Role { Admin, Customer}
    public class CreateUserCommand : IRequest<string>
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }    
        public Role? Role { get; set; }
    }
}
