namespace GradeMaster.Common.Entities;

public class Color
{
    public int Id
    {
        get; set;
    }

    public string Name
    {
        get; set;
    }

    public string Symbol
    {
        get; set;
    }

    public List<Note> Notes
    {
        get; set;
    } = [];
}