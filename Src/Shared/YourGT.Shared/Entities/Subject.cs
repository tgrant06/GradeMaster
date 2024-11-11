namespace YourGT.Shared.Entities;

public class Subject
{
    public int Id { get; set; }

    public string Name { get; set; }

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