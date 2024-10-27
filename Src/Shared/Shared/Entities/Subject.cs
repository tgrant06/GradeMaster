using Shared.Enums;

namespace Shared.Entities;

public class Subject
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Training Training { get; set; }
    public int TrainingId { get; set; }

    public Semester Semester { get; set; }
}