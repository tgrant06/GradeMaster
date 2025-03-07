namespace GradeMaster.Client.Shared.Utility;

/// <summary>
/// General utility class for UI.
/// </summary>
public static class UIUtils
{
    /// <summary>
    /// Truncates the given string to the given length.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="maxLength"></param>
    /// <returns>truncated string</returns>
    public static string TruncateString(string name, int maxLength)
    {
        if (string.IsNullOrEmpty(name) || name.Length <= maxLength)
        {
            return name;
        }

        return name.Substring(0, maxLength) + "...";
    }

    /// <summary>
    /// Returns the string of a descriptioon or a dash if empty
    /// </summary>
    /// <param name="input"></param>
    /// <param name="fallback"></param>
    /// <returns>string</returns>
    public static string FallbackIfEmpty(string input, string fallback) => string.IsNullOrEmpty(input) ? fallback : input;
}