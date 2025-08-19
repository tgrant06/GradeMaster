using System.Diagnostics;
using System.Text.Json;
using GradeMaster.DesktopClient.Json;

namespace GradeMaster.DesktopClient.Components.Pages;

public partial class Settings
{
    // [Inject]
    // private IDbContextUtilities DbContextUtilities { get; set; } = default!;

    #if DEBUG
        private const string AppName = "GradeMasterDev";
    #elif RELEASE
        private const string AppName = "GradeMaster";
    #endif

    private bool _isDatabaseStoredLocal = true;

    private bool _isDbStoredOneDrive;

    private bool _disabled;

    public bool Disabled => _disabled ? _disabled /*true*/ : _appPreferences.SaveDbFileToOneDriveLocation == _isDbStoredOneDrive;

    private AppPreferencesObject _appPreferences = new();

    private readonly string _appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName, "Data");

    private readonly string _oneDriveDataPath = Path.Combine(Environment.GetEnvironmentVariable("OneDrive") ??
                                                             Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OneDrive"), "Apps", AppName, "Data");

    private readonly string _appSettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName, "Data", "appPreferences.json");

    protected async override Task OnInitializedAsync()
    {
        if (File.Exists(_appSettingsFile))
        {
            var appPreferencesJsonString = await File.ReadAllTextAsync(_appSettingsFile);
            var currentAppPreferences = JsonSerializer.Deserialize(appPreferencesJsonString, AppJsonContext.Default.AppPreferencesObject);
            _appPreferences = currentAppPreferences ?? new AppPreferencesObject();

            _isDatabaseStoredLocal = !_appPreferences.SaveDbFileToOneDriveLocation;
            _isDbStoredOneDrive = _appPreferences.SaveDbFileToOneDriveLocation;
        }
    }

    private async Task SaveAppPreferences()
    {
        _appPreferences.SaveDbFileToOneDriveLocation = _isDbStoredOneDrive;

        _disabled = true;

        const string dbName = "GradeMaster.db";

        var localDb = Path.Combine(_appDataPath, dbName);
        var oneDriveDb = Path.Combine(_oneDriveDataPath, dbName);

        if (!Directory.Exists(_oneDriveDataPath)) Directory.CreateDirectory(_oneDriveDataPath);

        // copy existing db file if not existing
        if (_appPreferences.SaveDbFileToOneDriveLocation) // copy from local data directory
        {
            if (!File.Exists(oneDriveDb)) File.Copy(localDb, oneDriveDb);
        }
        else
        {
            if (!File.Exists(localDb)) File.Copy(oneDriveDb, localDb);
        }

        string tempFile = _appSettingsFile + ".tmp";

        // write to temp file
        await using (var fileStream = File.Create(tempFile))
        {
            await JsonSerializer.SerializeAsync(
                fileStream,
                _appPreferences,
                AppJsonContext.Default.AppPreferencesObject
            );
        }

        // replace original atomically
        File.Copy(tempFile, _appSettingsFile, overwrite: true);
        File.Delete(tempFile);

        await Task.Delay(100);

        RestartWindows();
    }

    private static void RestartWindows()
    {
        // Start a new instance
        Process.Start(Environment.ProcessPath!);

        // Quit the current app
        Application.Current?.Quit();
    }

    // async Task DisconnectDb()
    // {
    //     await DbContextUtilities.DisconnectFromDbAsync();
    // }

    // async Task ConnectToDb()
    // {
    //     await DbContextUtilities.ConnectToDbAsync();
    // }

    // async Task DisposeDbContext()
    // {
    //     await DbContextUtilities.DisposeDbContextAsync();
    // }
}