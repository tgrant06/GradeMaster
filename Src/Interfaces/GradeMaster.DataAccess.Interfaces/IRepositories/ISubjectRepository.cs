using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.GenericInterfaces;

namespace GradeMaster.DataAccess.Interfaces.IRepositories;

public interface ISubjectRepository : IGenericEntityRepository<Subject>
{
    Task<List<Subject>> GetByEducationIdAsync(int educationId);

    Task<List<Subject>> GetByEducationIdOrderedAsync(int educationId);

    Task<List<Subject>> GetAllWithGradesAsync();

    Task<Subject?> GetByIdDetailAsync(int id);

    //Task<List<Subject>> GetBySemesterAsync(int semester);

    Task<List<Subject>> GetByCompletedAsync(bool completed);

    Task<List<Subject>> GetByEducationIdAndCompletedAsync(int educationId , bool completed); 

    Task<List<Subject>> GetByEducationIdAndCompletedWithSemesterAsync(int educationId, bool completed, int semester);

    Task<Subject?> GetByGradeIdAsync(int gradeId);

    Task<List<Subject>> GetBySearchWithRangeAsync(string searchValue, int startIndex, int amount);

    Task<int> GetTotalCountAsync(string searchValue);

    Task<bool> ExistsAnyIsCompletedAsync(bool completed);

    Task<bool> ExistsAnyIsCompletedWithEducationIdAsync(int educationId, bool completed);

    Task<bool> ExistsAnyWithSpecifiedEducationIdNameSemester(int educationId, string subjectName, int subjectSemester);

    Task<int> ExistsAnyWithSpecifiedEducationIdNameSemesterAsSubjectId(int educationId, string subjectName, int subjectSemester);
}