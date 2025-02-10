using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.GenericInterfaces;

namespace GradeMaster.DataAccess.Interfaces.IRepositories;

public interface IWeightRepository : IGenericEntityRepository<Weight>
{
    // Todo GetByIds()
}