namespace YourGT.Shared.Entities;

public class Weight
{
    public int Id { get; set; }
    // divide at the end for example: (6+(4/2))/1.5
    // 1.5 = 6 Value + 4 Value
    // Grades = 6 Value is 1 | 4 Value is 0.5
    // Grades = 6 Divider Value is 1 | 4 Divider Value is 2
    public decimal Value { get; set; }

    public int DividerValue { get; set; }

    // if Multiply is True then this (6+(4/2))/1.5 becomes to this (6+(4*2))/3
    public bool Multiply { get; set; }
}