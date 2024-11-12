using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace YourGT.DataAccess.EFCore.Factories
{
    public class YourGTDbContextFactory : IDesignTimeDbContextFactory<YourGTDbContext>
    {
        public YourGTDbContext CreateDbContext(string[] args)
        {
            // Set up the configuration to read from appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())  // Current directory of the project
                .AddJsonFile("appsettings.json", optional: false)  // Load the configuration file
                .Build();

            // Retrieve the connection string from appsettings.json
            string connectionString = configuration.GetConnectionString("Default");

            // Create DbContextOptionsBuilder and configure it to use SQLite
            var optionsBuilder = new DbContextOptionsBuilder<YourGTDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            // Return a new instance of YourGTDbContext with the configured options
            return new YourGTDbContext(configuration);
        }
    }
}