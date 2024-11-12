namespace YourGT.Shared.GenericInterfaces;

public interface IGenericEntityRepository<T>
{
    T? GetById(int id);

    List<T> GetAll();

    T Add(T t);

    void Update(int id, T t);

    void DeleteById(int id);
}