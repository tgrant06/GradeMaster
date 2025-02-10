using GradeMaster.DataAccess;
using GradeMaster.DataAccess.Repositories;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GradeMaster.DesktopClient;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddBlazorBootstrap();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
        
        #region DbContext

        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())  // Ensure it's set to the current directory
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        // Register DbContext with connection string from appsettings.json
        builder.Services.AddDbContext<GradeMasterDbContext>(options =>
        {
            #if RELEASE
                var appName = "GradeMaster";
            #elif DEBUG
                var appName = "GradeMasterDev";
            #endif

            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName, "Data");

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            // Retrieve the connection string from configuration
            var connectionString = builder.Configuration.GetConnectionString("Default");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection string 'Default' is not found in appsettings.json.");
            }

            var fullConnectionString = Path.Combine(appDataPath, connectionString);

            options.UseSqlite($"Data Source={fullConnectionString}")
                .EnableDetailedErrors();
        });

        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<GradeMasterDbContext>();
            dbContext.Database.Migrate();  // Apply migrations here
        }

        #endregion

        #region Repositories

        builder.Services.AddScoped<IEducationRepository, EducationRepository>();
        builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
        builder.Services.AddScoped<IGradeRepository, GradeRepository>();
        builder.Services.AddScoped<IWeightRepository, WeightRepository>();

        #endregion

        return builder.Build();
    }
}
