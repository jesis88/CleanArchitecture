using Domain.Models;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]
    public class CQRSDbContext : IdentityDbContext<ApplicationUser, IdentityRole<string>, string>
    {
        public CQRSDbContext(DbContextOptions<CQRSDbContext> contextOptions) : base(contextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
            .Property(e => e.User.Id)
            .ValueGeneratedOnAdd();
        }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public override DbSet<ApplicationUser> Users { get; set; }
    }
}
