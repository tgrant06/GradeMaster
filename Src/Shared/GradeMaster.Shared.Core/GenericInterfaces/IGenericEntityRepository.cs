namespace GradeMaster.Shared.Core.GenericInterfaces;

public interface IGenericEntityRepository<T>
{
    Task<T?> GetByIdAsync(int id);

    Task<List<T>> GetAllAsync();

    Task<T> AddAsync(T t);

    void UpdateAsync(int id, T t);

    void DeleteByIdAsync(int id);
}