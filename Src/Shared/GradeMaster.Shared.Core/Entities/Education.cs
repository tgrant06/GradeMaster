namespace GradeMaster.Shared.Core.Entities;

public class Education
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    // length of Education in Semesters
    public int Semesters { get; set; }

    public bool Completed { get; set; }

    public DateTime StartDate { get; set; } // start date of education

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