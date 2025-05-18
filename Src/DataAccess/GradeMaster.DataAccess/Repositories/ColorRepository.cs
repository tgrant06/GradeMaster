using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GradeMaster.DataAccess.Repositories;

public class ColorRepository : IColorRepository
{
    private readonly GradeMasterDbContext _context;

    public ColorRepository(GradeMasterDbContext context)
    {
        _context = context;
    }

    public async Task<Color?> GetByIdAsync(int id)
    {
        return await _context.Colors.FindAsync(id);
    }

    public async Task<List<Color>> GetAllAsync()
    {
        return await _context.Colors.ToListAsync();
    }

    public async Task<Color> AddAsync(Color color)
    {
        _context.Notes.AttachRange(color.Notes);

        await _context.Colors.AddAsync(color);

        await _context.SaveChangesAsync();

        return color;
    }

    public async Task UpdateAsync(int id, Color color)
    {
        _context.Notes.AttachRange(color.Notes);

        _context.Colors.Update(color);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var existingColor = await GetByIdAsync(id);

        if (existingColor != null)
        {
            _context.Colors.Remove(existingColor);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAnyAsync()
    {
        return await _context.Colors.AnyAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Colors.AnyAsync(c => c.Id == id);
    }
}