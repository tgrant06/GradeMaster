using GradeMaster.Shared.Core.Entities;
using GradeMaster.Shared.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GradeMaster.DataAccess.Core.Repositories;

public class WeightRepository : IWeightRepository
{
    private readonly GradeMasterDbContext _context;

    public WeightRepository(GradeMasterDbContext context)
    {
        _context = context;
    }

    public async Task<Weight?> GetByIdAsync(int id)
    {
        return await _context.Weights.FindAsync(id);
    }

    public async Task<List<Weight>> GetAllAsync()
    {
        return await _context.Weights.ToListAsync();
    }

    public async Task<Weight> AddAsync(Weight weight)
    {
        _context.Grades.AttachRange(weight.Grades);

        await _context.Weights.AddAsync(weight);

        await _context.SaveChangesAsync();

        return weight;
    }

    public void UpdateAsync(int id, Weight weight)
    {
        _context.Grades.AttachRange(weight.Grades);

        _context.Weights.Update(weight);

        _context.SaveChangesAsync();
    }

    public void DeleteByIdAsync(int id)
    {
        var existingWeight = GetByIdAsync(id);

        if (existingWeight != null)
        {
            _context.Weights.Remove(existingWeight.Result);
            _context.SaveChanges();
        }
    }
}