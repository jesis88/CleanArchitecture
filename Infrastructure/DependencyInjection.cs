using Domain.Models;
using Infrastructure.MappingProfiles;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddDbContext<CQRSDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("CleanArchitecture"));
                options.EnableSensitiveDataLogging();
            });

            services.AddIdentity<ApplicationUser, IdentityRole<string>>()
                .AddRoles<IdentityRole<string>>()
                .AddEntityFrameworkStores<CQRSDbContext>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(ApplicationUserProfile).Assembly);
            return services;
        }
    }
}
