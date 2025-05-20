using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.GenericInterfaces;

namespace GradeMaster.DataAccess.Interfaces.IRepositories;

public interface INoteRepository : IGenericEntityRepository<Note>
{
    Task<List<Note>> GetBySearchWithRangeAsync(string searchValue, int startIndex, int amount);

    Task<int> GetTotalCountAsync(string searchValue);

    Task<Note?> GetByIdDetailAsync(int id);

    Task<int> GetTotalArchivedNotesCountAsync();
}