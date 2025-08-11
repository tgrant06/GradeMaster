namespace GradeMaster.Logic.Interfaces.GenericInterfaces;

public interface IGenericEntityService<in T>
{
    void PassObjectAttributes(T toObject, T fromObject, bool actionSubmit = false);
}