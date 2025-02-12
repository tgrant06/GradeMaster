using Microsoft.Extensions.Configuration;
using GradeMaster.DataAccess;

namespace GradeMaster.ConsoleClient;

public static class DbConnector
{
    public static GradeMasterDbContext Connect()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        IConfiguration config = builder.Build();

        //var context = new GradeMasterDbContext(config);

        return null;
    }
}