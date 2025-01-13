using GradeMaster.Shared.Core.Entities;
using GradeMaster.Shared.Core.GenericInterfaces;

namespace GradeMaster.Shared.Core.Interfaces;

public interface IEducationRepository : IGenericEntityRepository<Education>
{
    Task<Education?> GetBySubjectIdAsync(int id);

    Task<List<Education>> GetBySubjectIdsAsync(List<int> ids);
    // implement custom methods here, like searching by name, etc.
}