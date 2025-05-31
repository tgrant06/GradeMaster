namespace GradeMaster.Client.Shared.Utility;

/// <summary>
/// Note utility class.
/// </summary>
public static class NoteUtils
{
    /// <summary>
    /// Returns an array of tags from a comma-separated string.
    /// </summary>
    /// <param name="tags"></param>
    /// <returns>String Array of Tags</returns>
    public static string[] ToTagArray(string? tags)
    {
        return (tags ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim())
            .ToArray();
    }

    // Extension method version
    //public static string[] ToTagArray(this string tags)
    //{
    //    return (tags ?? "")
    //        .Split(',', StringSplitOptions.RemoveEmptyEntries)
    //        .Select(t => t.Trim())
    //        .ToArray();
    //}

    //public static void DoSomething()
    //{
    //    var str = "tag1, tag2, tag3";

    //    str.ToTagArray();
    //}
}