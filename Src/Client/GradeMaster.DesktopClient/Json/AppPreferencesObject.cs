namespace GradeMaster.DesktopClient.Json;

internal record AppPreferencesObject
{
    #if DEBUG
        private const string AppName = "GradeMasterDev";
    #elif RELEASE
        private const string AppName = "GradeMaster";
    #endif

    public bool SaveDbFileToOneDriveLocation { get; set; } = false;

    public string BackupLocalDirectoryLocation { get; init; } = string.Empty;

    public string BackupOneDriveDirectoryLocation { get; init; } = string.Empty;
}