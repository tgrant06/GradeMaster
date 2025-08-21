using System.Diagnostics;
using System.Text.Json;
using BlazorBootstrap;
using GradeMaster.DesktopClient.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Media;

namespace GradeMaster.DesktopClient.Components.Pages;

public partial class Settings
{
    // [Inject]
    // private IDbContextUtilities DbContextUtilities { get; set; } = default!;

    private ConfirmDialog _dialog = default!;

    [Inject] protected ToastService ToastService { get; set; } = default!;

    #if DEBUG
        private const string AppName = "GradeMasterDev";
    #elif RELEASE
        private const string AppName = "GradeMaster";
    #endif

    private const string DbName = "GradeMaster.db";

    private bool _isDatabaseStoredLocal = true;

    private bool _isDbStoredOneDrive;

    private bool _disabled;

    public bool Disabled => _disabled ? _disabled /*true*/ : _appPreferences.SaveDbFileToOneDriveLocation == _isDbStoredOneDrive;

    private AppPreferencesObject _appPreferences = new();

    private readonly string _appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName, "Data");

    private readonly string _oneDriveAppPath = Path.Combine(Environment.GetEnvironmentVariable("OneDrive") ??
                                                             Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OneDrive"), "Apps", AppName);

    private readonly string _oneDriveDataPath = Path.Combine(Environment.GetEnvironmentVariable("OneDrive") ??
                                                             Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OneDrive"), "Apps", AppName, "Data");

    private readonly string _appPreferencesFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName, "Data", "appPreferences.json");

    protected async override Task OnInitializedAsync()
    {
        if (File.Exists(_appPreferencesFile))
        {
            var appPreferencesJsonString = await File.ReadAllTextAsync(_appPreferencesFile);
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

        var localDb = Path.Combine(_appDataPath, DbName);
        var oneDriveDb = Path.Combine(_oneDriveDataPath, DbName);

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

        await UpdateAppPreferences(_appPreferences);

        await Task.Delay(50);

        RestartWindows();
    }

    private async Task OverrideDbAsync()
    {
        var options = new ConfirmDialogOptions
        {
            YesButtonColor = ButtonColor.Danger,
        };
        
        var confirmation = await _dialog.ShowAsync(
            title: "Are you sure you want to override a database?", 
            message1: "Data might be permanently lost. (Deleted data might still be in the trash bin)",
            confirmDialogOptions: options);

        if (confirmation)
        {
            try
            {
                OverrideSpecificDb();
                ToastService.Notify(new ToastMessage(ToastType.Success, $"Override successful."));
            }
            catch (Exception e)
            {
                ToastService.Notify(new ToastMessage(ToastType.Danger, "Override failed."));
            }
        }
        else
        {
            ToastService.Notify(new ToastMessage(ToastType.Secondary, $"Override action canceled."));
        }
    }

    private void OverrideSpecificDb()
    {
        _disabled = true;

        var localDb = Path.Combine(_appDataPath, DbName);
        var oneDriveDb = Path.Combine(_oneDriveDataPath, DbName);

        if (!Directory.Exists(_oneDriveDataPath)) Directory.CreateDirectory(_oneDriveDataPath);

        if (_appPreferences.SaveDbFileToOneDriveLocation) // copy from local data directory
        {
            File.Copy(oneDriveDb, localDb, overwrite: true);
        }
        else
        {
            File.Copy(localDb, oneDriveDb, overwrite: true);
        }

        _disabled = false;
    }

    private async Task CreateBackup()
    {
        _disabled = true;

        var localDb = Path.Combine(_appDataPath, DbName);
        var oneDriveDb = Path.Combine(_oneDriveDataPath, DbName);

        var backupLocalDir = _appPreferences.BackupLocalDirectoryLocation;
        var backupOneDriveDir = _appPreferences.BackupOneDriveDirectoryLocation;

        var machineName = Environment.MachineName;

        if (string.IsNullOrWhiteSpace(backupLocalDir))
        {
            backupLocalDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName, "Backup");
        }

        if (string.IsNullOrWhiteSpace(backupOneDriveDir))
        {
            backupOneDriveDir = Path.Combine(_oneDriveAppPath, "Backup");
        }

        if (backupLocalDir != _appPreferences.BackupLocalDirectoryLocation || backupOneDriveDir != _appPreferences.BackupOneDriveDirectoryLocation)
        {
            var updatedAppPreferences = _appPreferences with
            {
                BackupLocalDirectoryLocation = backupLocalDir,
                BackupOneDriveDirectoryLocation = backupOneDriveDir
            };

            await UpdateAppPreferences(updatedAppPreferences);
        }

        backupLocalDir = Path.Combine(backupLocalDir, machineName);
        backupOneDriveDir = Path.Combine(backupOneDriveDir, machineName);

        if (!Directory.Exists(backupLocalDir)) Directory.CreateDirectory(backupLocalDir);

        if (!Directory.Exists(backupOneDriveDir)) Directory.CreateDirectory(backupOneDriveDir);

        if (File.Exists(localDb))
        {
            File.Copy(localDb, Path.Combine(backupLocalDir, DbName), overwrite: true);
        }

        if (File.Exists(oneDriveDb))
        {
            File.Copy(oneDriveDb, Path.Combine(backupOneDriveDir, DbName), overwrite: true);
        }

        ToastService.Notify(new ToastMessage(ToastType.Success, "Backup(s) created successfully."));

        _disabled = false;
    }

    private static void RestartWindows()
    {
        // Start a new instance
        Process.Start(Environment.ProcessPath!);

        // Quit the current app
        Application.Current?.Quit();
    }

    private async Task UpdateAppPreferences(AppPreferencesObject appPreferences)
    {
        string tempFile = _appPreferencesFile + ".tmp";

        // write to temp file
        await using (var fileStream = File.Create(tempFile))
        {
            await JsonSerializer.SerializeAsync(
                fileStream,
                appPreferences,
                AppJsonContext.Default.AppPreferencesObject
            );
        }

        // replace original atomically
        File.Copy(tempFile, _appPreferencesFile, overwrite: true);
        File.Delete(tempFile);
    }

    #region Not used

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

    #endregion
}