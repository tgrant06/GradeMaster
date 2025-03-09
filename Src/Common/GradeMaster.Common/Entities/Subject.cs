using System.ComponentModel.DataAnnotations;

namespace GradeMaster.Common.Entities;

public class Subject
{
    public int Id { get; set; }

    [Required]
    [MaxLength(255, ErrorMessage = "Name may not exceed 255 characters.")]
    public string Name { get; set; }

    [MaxLength(2500, ErrorMessage = "Description may not exceed 2500 characters.")]
    public string? Description { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Semester must be at least 1.")]
    public int Semester { get; set; }

    [Required]
    public bool Completed { get; set; }
    // teacher? add Entity people, persons , teachers, students... users?

    // maybe add DateOnly Date { get; set; } // date for sorting? optional

    [Required]
    public Education Education { get; set; }
    public int EducationId { get; set; }

    public List<Grade> Grades
    {
        get; set;
    }

    public Subject()
    {
        Grades = new List<Grade>();
    }
}