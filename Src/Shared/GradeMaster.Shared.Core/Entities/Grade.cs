using System.ComponentModel.DataAnnotations;

namespace GradeMaster.Shared.Core.Entities;

public class Grade
{
    public int Id { get; set; }

    [Required]
    [Range(1, 6, ErrorMessage = "Value must be between 1 and 6.")]
    public decimal Value { get; set; } // 1 - 6

    [MaxLength(2500, ErrorMessage = "Description may not exceed 2500 characters.")]
    public string? Description { get; set; }

    [Required]
    public DateOnly Date { get; set;}

    #region Relations

    [Required]
    public Subject Subject { get; set; }
    public int SubjectId { get; set; }

    [Required]
    public Weight Weight { get; set; }
    public int WeightId { get; set; }

    #endregion
}