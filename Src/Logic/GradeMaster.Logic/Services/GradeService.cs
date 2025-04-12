using GradeMaster.Common.Entities;
using GradeMaster.Logic.Interfaces.IServices;

namespace GradeMaster.Logic.Services;

public class GradeService : IGradeService
{
    public void PassObjectAttributes(Grade toGrade, Grade fromGrade, bool actionSubmit = false)
    {
        ArgumentNullException.ThrowIfNull(fromGrade);

        ArgumentNullException.ThrowIfNull(toGrade);

        toGrade.Value = fromGrade.Value;
        toGrade.Description = actionSubmit ? fromGrade.Description?.Trim() : fromGrade.Description;
        toGrade.Date = fromGrade.Date;
        toGrade.Weight = fromGrade.Weight ?? default!;
        toGrade.Subject = fromGrade.Subject ?? default!;
    }
}