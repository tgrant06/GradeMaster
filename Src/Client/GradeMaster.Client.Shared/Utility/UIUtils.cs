namespace GradeMaster.Client.Shared.Utility;

public static class UIUtils
{
    public static string TruncateString(string name, int maxLength)
    {
        if (string.IsNullOrEmpty(name) || name.Length <= maxLength)
        {
            return name;
        }

        return name.Substring(0, maxLength) + "...";
    }
}