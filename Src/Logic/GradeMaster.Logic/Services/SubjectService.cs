using GradeMaster.Common.Entities;
using GradeMaster.Logic.Interfaces.IServices;

namespace GradeMaster.Logic.Services;

public class SubjectService : ISubjectService
{
    public void PassObjectAttributes(Subject toSubject, Subject fromSubject, bool actionSubmit = false)
    {
        ArgumentNullException.ThrowIfNull(fromSubject);

        ArgumentNullException.ThrowIfNull(toSubject);

        toSubject.Name = actionSubmit ? fromSubject.Name.Trim() : fromSubject.Name;
        toSubject.Description = actionSubmit ? fromSubject.Description?.Trim() : fromSubject.Description;
        toSubject.Semester = fromSubject.Semester;
        toSubject.Completed = fromSubject.Completed;
        toSubject.Education = fromSubject.Education ?? new Education();
    }
}