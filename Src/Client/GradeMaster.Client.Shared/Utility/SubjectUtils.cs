using GradeMaster.Common.Entities;

namespace GradeMaster.Client.Shared.Utility;

/// <summary>
/// Subject utility class.
/// </summary>
public static class SubjectUtils
{
    /// <summary>
    /// Calculates the weighted average of the given grades.
    /// </summary>
    /// <param name="grades"></param>
    /// <returns>weighted average as decimal</returns>
    public static decimal CalculateWeightedAverage(ICollection<Grade> grades)
    {
        if (grades == null || grades.Count == 0)
        {
            return 0;
        }

        decimal totalWeight = grades.Sum(g => g.Weight?.Value ?? 1); // Default weight to 1 if null
        if (totalWeight == 0)
        {
            return 0;
        }

        decimal weightedSum = grades.Sum(g => g.Value * (g.Weight?.Value ?? 1)); // Default weight to 1 if null
        return weightedSum / totalWeight;
    }

    ///// <summary>
    ///// Returns the completion state of the given boolean.
    ///// </summary>
    ///// <param name="completed"></param>
    ///// <returns>Completion state as a string</returns>
    //public static string CompletionState(bool completed)
    //{
    //    return completed ? "Completed" : "In Progress";
    //}
}