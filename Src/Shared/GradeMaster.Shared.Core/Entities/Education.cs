using System.ComponentModel.DataAnnotations;

namespace GradeMaster.Shared.Core.Entities;

public class Education
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    // length of Education in Semesters
    [Required]
    public int Semesters { get; set; }

    [Required]
    public bool Completed { get; set; }

    [Required]
    public DateTime StartDate { get; set; } // start date of education

    [Required]
    public DateTime EndDate { get; set; } // end date of education
    // public string Duration { get; set; } //start date and end date? optional

    // maybe implement duration / length?

    public string? Institution { get; set; } // optional

    public List<Subject> Subjects { get; set; }

    public Education()
    {
        Subjects = new List<Subject>();
    }
}