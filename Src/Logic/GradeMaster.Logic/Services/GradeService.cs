using GradeMaster.Common.Entities;
using GradeMaster.Logic.Interfaces.IServices;

namespace GradeMaster.Logic.Services;

public class GradeService : IGradeService
{
    public void PassObjectAttributes(Grade toGrade, Grade fromGrade, bool actionSubmit = false)
    {
        if (fromGrade is null)
        {
            throw new ArgumentNullException(nameof(fromGrade));
        }

        if (toGrade is null)
        {
            throw new ArgumentNullException(nameof(toGrade));
        }

        toGrade.Value = fromGrade.Value;
        toGrade.Description = actionSubmit ? fromGrade.Description?.Trim() : fromGrade.Description;
        toGrade.Date = fromGrade.Date;
        toGrade.Weight = fromGrade.Weight ?? new Weight();
        toGrade.Subject = fromGrade.Subject ?? new Subject();
    }
}