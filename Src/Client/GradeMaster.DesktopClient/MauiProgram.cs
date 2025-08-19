using System.Text.Json;
using GradeMaster.DataAccess;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using GradeMaster.DataAccess.Repositories;
using GradeMaster.DesktopClient.Json;
using GradeMaster.Logic.Interfaces.IServices;
using GradeMaster.Logic.Services;
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
            #if DEBUG
                const string appName = "GradeMasterDev";
            #elif RELEASE
                const string appName = "GradeMaster";
            #endif

            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName, "Data");

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            var appPreferencesFile = Path.Combine(appDataPath, "appPreferences.json");

            var oneDrivePath = Environment.GetEnvironmentVariable("OneDrive") ??
                               Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OneDrive");

            if (!File.Exists(appPreferencesFile))
            {
                var appPreferences = new AppPreferencesObject
                {
                    //Todo: Add Machine Name to the Backup paths
                    BackupOneDriveDirectoryLocation = Path.Combine(oneDrivePath, "Apps", appName, "Backup"),
                    BackupLocalDirectoryLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName, "Backup")
                };

                using var fileStream = File.Create(appPreferencesFile);
                JsonSerializer.Serialize(fileStream, appPreferences, AppJsonContext.Default.AppPreferencesObject);
            }

                // Retrieve the connection string from configuration
                var connectionString = builder.Configuration.GetConnectionString("Default");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection string 'Default' is not found in appsettings.json.");
            }

            var appPreferencesJsonString = File.ReadAllText(appPreferencesFile);

            var currentAppPreferences = JsonSerializer.Deserialize(appPreferencesJsonString, AppJsonContext.Default.AppPreferencesObject);

            string fullConnectionString;

            if (currentAppPreferences is null 
                || !currentAppPreferences.SaveDbFileToOneDriveLocation 
                || !currentAppPreferences.SaveDbFileToOneDriveLocation 
                /*|| string.IsNullOrWhiteSpace(oneDrivePath)*/)
            {
                fullConnectionString = Path.Combine(appDataPath, connectionString);
            }
            else
            {
                var fullOneDrivePath = Path.Combine(oneDrivePath, "Apps", appName, "Data");

                if (!Directory.Exists(fullOneDrivePath))
                {
                    Directory.CreateDirectory(fullOneDrivePath);
                }

                // maybe copy local db if already exists and copy to onedrive
                fullConnectionString = Path.Combine(fullOneDrivePath, connectionString);
            }

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
        builder.Services.AddScoped<INoteRepository, NoteRepository>();
        builder.Services.AddScoped<IColorRepository, ColorRepository>();
        //builder.Services.AddTransient<IDbContextUtilities, DbContextUtilities>();

        #endregion

        #region Services

        builder.Services.AddTransient<IEducationService, EducationService>();
        builder.Services.AddTransient<ISubjectService, SubjectService>();
        builder.Services.AddTransient<IGradeService, GradeService>();
        builder.Services.AddTransient<INoteService, NoteService>();

        #endregion

        return builder.Build();
    }
}
