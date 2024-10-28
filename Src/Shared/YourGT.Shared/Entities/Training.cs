namespace YourGT.Shared.Entities;

public class Training
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }


    // length of Training
    public int Semesters { get; set; }

    // maybe implement duration / length?
}