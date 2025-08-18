namespace GradeMaster.DesktopClient.Json;

internal record AppPreferencesObject
{
    public bool SaveDbFileToOneDriveLocation { get; set; } = false;

    public string SnapshotDirectoryLocation { get; set; } = string.Empty;
}