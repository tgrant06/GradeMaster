using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GradeMaster.DataAccess.Factories
{
    public class GradeMasterDbContextFactory : IDesignTimeDbContextFactory<GradeMasterDbContext>
    {
        public GradeMasterDbContext CreateDbContext(string[] args)
        {
            // Set up the configuration to read from appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())  // Current directory of the project
                .AddJsonFile("appsettings.json", optional: false)  // Load the configuration file
                .Build();


            var connectionString = configuration.GetConnectionString("Default");

            // Set the app data path (LocalApplicationData folder)
            var appName = "GradeMaster";
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName);

            // Ensure the directory exists
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            // Build the full connection string with the local app data path
            var fullConnectionString = $"Data Source={Path.Combine(appDataPath, connectionString)}";

            // Build the DbContextOptions using the connection string
            var optionsBuilder = new DbContextOptionsBuilder<GradeMasterDbContext>();
            optionsBuilder.UseSqlite(fullConnectionString)
                .EnableDetailedErrors();

            return new GradeMasterDbContext(optionsBuilder.Options);
        }
    }
}