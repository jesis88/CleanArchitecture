using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class User
    {
        public required string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Role? Role { get; set; }
        public DateTime? RecentLogin { get; set; }
    }

    public enum Role
    {
        Admin,
        Customer
    }
}
