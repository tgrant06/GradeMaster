namespace YourGT.Shared.Entities;

public class Education
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    // length of Education in Semesters
    public int Semesters { get; set; }

    public bool Completed { get; set; }
    
    // maybe implement duration / length?

    public List<Subject> Subjects { get; set; }

    public Education()
    {
        Subjects = new List<Subject>();
    }
}