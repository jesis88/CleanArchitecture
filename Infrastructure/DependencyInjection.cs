using Application.Interfaces;
using AutoMapper;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.MappingProfiles;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.KafkaServices;
using Infrastructure.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CqrsDbContext>(options =>
             {
                 System.Diagnostics.Debug.WriteLine($"PSQL_PASSWORD: {Environment.GetEnvironmentVariable("PSQL_PASSWORD")}");

                 var connectionStringBuilder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("CleanArchitecture"))
                 {
                     Password = Environment.GetEnvironmentVariable("PSQL_PASSWORD")
                 };
                 options.UseNpgsql(connectionStringBuilder.ConnectionString, b => b.MigrationsAssembly("Infrastructure"));
                 options.EnableSensitiveDataLogging();
             });

            services.AddIdentity<ApplicationUser, IdentityRole<string>>()
                .AddRoles<IdentityRole<string>>()
                .AddEntityFrameworkStores<CqrsDbContext>()
                .AddDefaultTokenProviders();

            services.AddHangfire(x => x.UsePostgreSqlStorage(options =>
            {
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("CleanArchitecture"))
                {
                    Password = Environment.GetEnvironmentVariable("PSQL_PASSWORD")
                };
                options.UseNpgsqlConnection(connectionStringBuilder.ConnectionString);
            }));
            services.AddHangfireServer();

            services.AddScoped<IUserManagerWrapper, UserManagerWrapper>();
            services.AddScoped<IRoleManagerWrapper, RoleManagerWrapper>();
            services.AddScoped<ICustomerWrapper, CustomerWrapper>();
            services.AddScoped<IOrderWrapper, OrderWrapper>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddSingleton<KafkaProducer>();
            services.AddSingleton<KafkaConsumer>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApplicationUserProfile());
                // Add other profiles if any
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton<IRefreshTokenService, RefreshTokenService>();
            return services;
        }
    }
}
