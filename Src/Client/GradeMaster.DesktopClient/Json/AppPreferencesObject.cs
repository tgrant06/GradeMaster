namespace GradeMaster.DesktopClient.Json;

internal record AppPreferencesObject
{
    #if DEBUG
        private const string AppName = "GradeMasterDev";
    #elif RELEASE
        private const string AppName = "GradeMaster";
    #endif

    public bool SaveDbFileToOneDriveLocation { get; set; } = false;

    public string BackupLocalDirectoryLocation { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName, "Backup");

    public string BackupCustomDirectoryLocation { get; set; } = string.Empty;
}