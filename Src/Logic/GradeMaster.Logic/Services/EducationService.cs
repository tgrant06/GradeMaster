using GradeMaster.Common.Entities;
using GradeMaster.Logic.Interfaces.IServices;

namespace GradeMaster.Logic.Services;

public class EducationService : IEducationService
{
    public void PassObjectAttributes(Education toEducation, Education fromEducation, bool actionSubmit = false)
    {
        ArgumentNullException.ThrowIfNull(fromEducation);

        ArgumentNullException.ThrowIfNull(toEducation);

        toEducation.Name = actionSubmit ? fromEducation.Name.Trim() : fromEducation.Name;
        toEducation.Description = actionSubmit ? fromEducation.Description?.Trim() : fromEducation.Description;
        toEducation.Institution = actionSubmit ? fromEducation.Institution?.Trim() : fromEducation.Institution;
        toEducation.Completed = fromEducation.Completed;
        toEducation.Semesters = fromEducation.Semesters;
        toEducation.StartDate = fromEducation.StartDate;
        toEducation.EndDate = fromEducation.EndDate;
    }
}