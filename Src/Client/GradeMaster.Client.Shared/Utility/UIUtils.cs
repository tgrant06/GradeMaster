namespace GradeMaster.Client.Shared.Utility;

public static class UIUtils
{
    /// <summary>
    /// Truncates the given string to the given length.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="maxLength"></param>
    /// <returns>string</returns>
    public static string TruncateString(string name, int maxLength)
    {
        if (string.IsNullOrEmpty(name) || name.Length <= maxLength)
        {
            return name;
        }

        return name.Substring(0, maxLength) + "...";
    }
}