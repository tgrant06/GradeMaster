using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.GenericInterfaces;

namespace GradeMaster.DataAccess.Interfaces.IRepositories;

public interface IEducationRepository : IGenericEntityRepository<Education>
{
    Task<Education?> GetBySubjectIdAsync(int id);

    //Task<List<Education>> GetBySubjectIdsAsync(List<int> ids);
    // implement custom methods here, like searching by name, etc.

    Task<List<Education>> GetByCompletedAsync(bool completed);

    Task<List<Education>> GetBySearchWithLimitAsync(string searchValue, int startIndex, int amount);

    Task<int> GetTotalCountAsync(string searchValue);

    Task<List<Education>> GetAllSimpleAsync();

    //Task<List<Education>> GetBySemestersAsync(int semesters); later

    Task<bool> ExistsAnyIsCompletedAsync(bool completed);
}