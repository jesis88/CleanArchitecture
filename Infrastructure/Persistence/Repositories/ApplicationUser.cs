using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ApplicationUser : IdentityUser<string>
    {
        public required User User { get; set; }

        [NotMapped]
        public string RefreshToken { get; set; } = string.Empty;

        [NotMapped]
        public DateTime TokenCreated { get; set; }

        [NotMapped]
        public DateTime TokenExpires { get; set; }
    }
}
