using Domain.Entity;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]
    public class CqrsDbContext(DbContextOptions<CqrsDbContext> contextOptions) : IdentityDbContext<ApplicationUser, IdentityRole<string>, string>(contextOptions)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>()
            .HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Customer>(c => c.UserId);

            builder.Entity<Order>()
                .HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId);
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders {  get; set; }

        public override DbSet<ApplicationUser> Users { get; set; }
    }
}
