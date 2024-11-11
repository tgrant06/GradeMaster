namespace YourGT.Shared.Interfaces;

public interface IGenericEntityRepository<T>
{
    T GetById(int id);

    List<T> GetAll();

    void Add(T t);

    void Update(int id, T t);

    void DeleteById(int id);
}