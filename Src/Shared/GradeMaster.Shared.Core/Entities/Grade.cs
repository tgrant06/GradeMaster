using System.ComponentModel.DataAnnotations;

namespace GradeMaster.Shared.Core.Entities;

public class Grade
{
    public int Id { get; set; }

    [Required]
    public decimal Value { get; set; } // 1 - 6

    public string? Description { get; set; }

    [Required]
    public DateTime CreatedAt { get; set;}

    #region Relations

    [Required]
    public Subject Subject { get; set; }
    public int SubjectId { get; set; }

    [Required]
    public Weight Weight { get; set; }
    public int WeightId { get; set; }

    #endregion
}