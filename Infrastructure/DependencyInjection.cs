using Application.Interfaces;
using CQRSApplication.RoleManagerWrappers;
using CQRSApplication.UserManagerWrappers;
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
            services.AddDbContext<CQRSDbContext>(options =>
             {
                 options.UseNpgsql(configuration.GetConnectionString("CleanArchitecture"), b => b.MigrationsAssembly("Infrastructure"));
                 options.EnableSensitiveDataLogging();
             });

            services.AddIdentity<ApplicationUser, IdentityRole<string>>()
                .AddRoles<IdentityRole<string>>()
                .AddEntityFrameworkStores<CQRSDbContext>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(ApplicationUserProfile).Assembly);

            services.AddScoped<IUserManagerWrapper, UserManagerWrapper>();
            services.AddScoped<IRoleManagerWrapper, RoleManagerWrapper>();
            return services;
        }
    }
}
