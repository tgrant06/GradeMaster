using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GradeMaster.DataAccess.Repositories;

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

    public async Task UpdateAsync(int id, Weight weight)
    {
        _context.Grades.AttachRange(weight.Grades);

        _context.Weights.Update(weight);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var existingWeight = await GetByIdAsync(id);

        if (existingWeight != null)
        {
            _context.Weights.Remove(existingWeight);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAnyAsync()
    {
        return await _context.Weights.AnyAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Weights.AnyAsync(w => w.Id == id);
    }
}