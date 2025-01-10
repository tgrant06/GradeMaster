using System.ComponentModel.DataAnnotations;

namespace GradeMaster.Shared.Core.Entities;

public class Grade
{
    public int Id { get; set; }

    [Required]
    [Range(1, 6, ErrorMessage = "Value must be between 1 and 6.")]
    public decimal Value { get; set; } // 1 - 6

    public string? Description { get; set; }

    [Required]
    public DateTime CreatedAt { get; set;}
    // change name to Date

    #region Relations

    [Required]
    public Subject Subject { get; set; }
    public int SubjectId { get; set; }

    [Required]
    public Weight Weight { get; set; }
    public int WeightId { get; set; }

    #endregion
}