using System.ComponentModel.DataAnnotations;

namespace GradeMaster.Shared.Core.Entities;

public class Subject
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public int Semester { get; set; }

    [Required]
    public bool Completed { get; set; }
    // teacher? add Entity people, persons , teachers, students... users?

    [Required]
    public Education Education { get; set; }
    public int EducationId { get; set; }

    public ICollection<Grade> Grades
    {
        get; set;
    }

    public Subject()
    {
        Grades = new List<Grade>();
    }
}