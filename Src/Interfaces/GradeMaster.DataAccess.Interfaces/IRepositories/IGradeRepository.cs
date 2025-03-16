using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.GenericInterfaces;

namespace GradeMaster.DataAccess.Interfaces.IRepositories;

public interface IGradeRepository : IGenericEntityRepository<Grade>
{
    Task<List<Grade>> GetBySubjectIdsAsync(List<int> id);

    Task<List<Grade>> GetBySubjectIdAsync(int id);

    Task<List<Grade>> GetBySubjectIdSimpleAsync(int id);

    Task<Grade?> GetByIdDetailAsync(int id);

    Task<List<Grade>> GetAllWithWeightAsync();

    Task<List<Grade>> GetBySearchWithRangeAsync(string searchValue, int startIndex, int amount);

    Task<int> GetTotalCountAsync(string searchValue);
}