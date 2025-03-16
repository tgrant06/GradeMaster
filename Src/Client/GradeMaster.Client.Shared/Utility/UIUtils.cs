namespace GradeMaster.Client.Shared.Utility;

/// <summary>
/// General utility class for UI.
/// </summary>
public static class UIUtils
{
    /// <summary>
    /// Truncates the given string to the given length.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="maxLength"></param>
    /// <returns>truncated string</returns>
    public static string TruncateString(string input, int maxLength)
    {
        if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
        {
            return input;
        }

        return input[..maxLength] + "...";
    }

    /// <summary>
    /// Truncates the given string to the given length.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="fallback"></param>
    /// <param name="maxLength"></param>
    /// <returns>truncated string or fallback</returns>
    public static string TruncateStringWithFallback(string? input, string fallback, int maxLength)
    {
        if (string.IsNullOrEmpty(input))
        {
            return fallback;
        }

        if (input.Length <= maxLength)
        {
            return input;
        }

        return input[..maxLength] + "...";
    }

    /// <summary>
    /// Returns the string or a dash if empty
    /// </summary>
    /// <param name="input"></param>
    /// <param name="fallback"></param>
    /// <returns>string</returns>
    public static string FallbackIfEmpty(string? input, string fallback) => string.IsNullOrEmpty(input) ? fallback : input;
}