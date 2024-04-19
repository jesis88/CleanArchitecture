using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CqrsDbContext>
    {
        public CqrsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CqrsDbContext>();
            var dbPassword = Environment.GetEnvironmentVariable("PSQL_PASSWORD");
            optionsBuilder.UseNpgsql($"Server=localhost;Database=CA;Username=postgres;Password={dbPassword}");

            return new CqrsDbContext(optionsBuilder.Options);
        }
    }
}
