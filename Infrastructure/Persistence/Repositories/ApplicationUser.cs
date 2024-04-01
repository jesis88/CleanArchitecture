using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Repositories
{
    public class ApplicationUser : IdentityUser<string>
    {
        public DateTime? RecentLogin { get; set; }
    }
}
