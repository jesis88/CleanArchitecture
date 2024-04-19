using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly);

            services.AddStackExchangeRedisCache(redisOptions =>
            {
                string? connection = configuration.GetConnectionString("Redis");
                if (connection != null)
                {
                    redisOptions.Configuration = connection;
                }
                else
                {
                    throw new InvalidOperationException("Redis connection string is null");
                }
            });
            return services;
        }
    }
}
