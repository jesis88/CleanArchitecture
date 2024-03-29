using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class User
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public required DateTime RecentLogin { get; set; }
    }   
}
