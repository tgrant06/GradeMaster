namespace GradeMaster.Shared.Core.Entities;

public class Weight
{
    public int Id { get; set; }

    public string Name { get; set; } // For Example 50%

    #region Example
    // divide at the end for example: (6*1)+(4*0.5))/1.5
    // 1.5 = 6 Value + 4 Value
    // Grades = 6 Value is 1 | 4 Value is 0.5
    #endregion
    public decimal Value { get; set; } // weight of the grade

    public ICollection<Grade> Grades
    {
        get; set;
    }

    public Weight()
    {
        Grades = new List<Grade>();
    }
}