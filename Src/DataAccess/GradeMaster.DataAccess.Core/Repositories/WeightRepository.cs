using GradeMaster.Shared.Core.Entities;
using GradeMaster.Shared.Core.Interfaces;

namespace GradeMaster.DataAccess.Core.Repositories;

public class WeightRepository : IWeightRepository
{
    private readonly GradeMasterDbContext _context;

    public WeightRepository(GradeMasterDbContext context)
    {
        _context = context;
    }

    public Weight? GetById(int id)
    {
        return _context.Weights.Find(id);
    }

    public List<Weight> GetAll()
    {
        return _context.Weights.ToList();
    }

    public Weight Add(Weight weight)
    {
        _context.Grades.AttachRange(weight.Grades);

        _context.Weights.Add(weight);

        _context.SaveChanges();

        return weight;
    }

    public void Update(int id, Weight weight)
    {
        _context.Grades.AttachRange(weight.Grades);

        _context.Weights.Update(weight);

        _context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var existingWeight = GetById(id);

        if (existingWeight != null)
        {
            _context.Weights.Remove(existingWeight);
            _context.SaveChanges();
        }
    }
}