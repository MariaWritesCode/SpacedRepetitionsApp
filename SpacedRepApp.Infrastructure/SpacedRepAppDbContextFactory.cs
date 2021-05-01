using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacedRepApp.Infrastructure
{
    public class SpacedRepAppDbContextFactory : IDesignTimeDbContextFactory<SpacedRepAppDbContext>
    {
        private const string ConnectionStringName = "SpacedRepAppDb";
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        public SpacedRepAppDbContext CreateDbContext(string[] args)
        {
            return Create(
             Directory.GetCurrentDirectory(),
             Environment.GetEnvironmentVariable(AspNetCoreEnvironment));
        }

        protected SpacedRepAppDbContext CreateNewInstance(DbContextOptions<SpacedRepAppDbContext> options)
        {
            return new SpacedRepAppDbContext(options);
        }

        private SpacedRepAppDbContext Create(string basePath, string environmentName)
        {
            var configuration = new ConfigurationBuilder()
             .SetBasePath(basePath)
             .AddJsonFile("appsettings.json")
             .AddJsonFile($"appsettings.Local.json", optional: true)
             .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
             .Build();

            var connectionString = configuration.GetConnectionString(ConnectionStringName);

            return Create(connectionString);
        }

        private SpacedRepAppDbContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"Connection string '{ConnectionStringName}' is null or empty.", nameof(connectionString));
            }

            Console.WriteLine($"DesignTimeDbContextFactoryBase.Create(string): Connection string: '{connectionString}'.");

            var optionsBuilder = new DbContextOptionsBuilder<SpacedRepAppDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return CreateNewInstance(optionsBuilder.Options);
        }
    }
}

