using GradeMaster.Shared.Core.Entities;
using GradeMaster.Shared.Core.GenericInterfaces;

namespace GradeMaster.Shared.Core.Interfaces;

public interface IGradeRepository : IGenericEntityRepository<Grade>
{
    Task<List<Grade>> GetBySubjectIdsAsync(List<int> id);

    Task<List<Grade>> GetBySubjectIdAsync(int id);

    Task<Grade?> GetByIdDetailAsync(int id);

    Task<List<Grade>> GetAllWithWeightAsync();

    Task<List<Grade>> GetBySearchWithLimitAsync(string searchValue, int startIndex, int amount);

    Task<int> GetTotalCountAsync(string searchValue);
}