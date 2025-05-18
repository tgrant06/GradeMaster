using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.GenericInterfaces;

namespace GradeMaster.DataAccess.Interfaces.IRepositories;

public interface IColorRepository : IGenericEntityRepository<Color>
{
    // Todo GetByIds()
}