using GradeMaster.Shared.Core.Entities;
using GradeMaster.Shared.Core.GenericInterfaces;

namespace GradeMaster.Shared.Core.Interfaces;

public interface ISubjectRepository : IGenericEntityRepository<Subject>
{
    Task<List<Subject>> GetByEducationIdAsync(int id);

    Task<List<Subject>> GetAllWithGradesAsync();

    Task<Subject?> GetByIdDetailAsync(int id);

    //Task<List<Subject>> GetBySemesterAsync(int semester);

    Task<List<Subject>> GetByCompletedAsync(bool completed);

    Task<Subject?> GetByGradeIdAsync(int id);

    Task<List<Subject>> GetBySearchWithLimitAsync(string searchValue, int startIndex, int amount);

    Task<int> GetTotalCountAsync(string searchValue);
}