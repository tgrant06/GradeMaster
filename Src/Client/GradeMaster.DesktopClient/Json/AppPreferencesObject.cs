namespace GradeMaster.DesktopClient.Json;

internal record AppPreferencesObject
{
    public bool SaveDbFileToOneDriveLocation { get; set; } = false;

    public string BackupLocalDirectoryLocation { get; init; } = string.Empty;

    public string BackupOneDriveDirectoryLocation { get; init; } = string.Empty;
}