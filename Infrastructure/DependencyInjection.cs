using Application.Interfaces;
using AutoMapper;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.MappingProfiles;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Infrastructure.Wrappers;
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

            services.AddHangfire(x => x.UsePostgreSqlStorage(options =>
            {
                options.UseNpgsqlConnection(configuration.GetConnectionString("CleanArchitecture"));
            }));
            services.AddHangfireServer();

            services.AddScoped<IUserManagerWrapper, UserManagerWrapper>();
            services.AddScoped<IRoleManagerWrapper, RoleManagerWrapper>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApplicationUserProfile());
                // Add other profiles if any
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}
