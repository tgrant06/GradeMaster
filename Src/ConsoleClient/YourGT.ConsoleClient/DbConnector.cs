using Microsoft.Extensions.Configuration;
using YourGT.DataAccess.EFCore;

namespace YourGT.ConsoleClient;

public static class DbConnector
{
    public static YourGTDbContext Connect()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        IConfiguration config = builder.Build();

        var context = new YourGTDbContext(config);

        return context;
    }
}