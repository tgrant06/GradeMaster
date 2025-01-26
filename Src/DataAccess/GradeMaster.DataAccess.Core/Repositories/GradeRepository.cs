using Microsoft.EntityFrameworkCore;
using GradeMaster.Shared.Core.Entities;
using GradeMaster.Shared.Core.Interfaces;

namespace GradeMaster.DataAccess.Core.Repositories;

public class GradeRepository : IGradeRepository
{
    private readonly GradeMasterDbContext _context;

    public GradeRepository(GradeMasterDbContext context)
    {
        _context = context;
    }

    public async Task<Grade?> GetByIdAsync(int id)
    {
        return await _context.Grades.FindAsync(id);
    }

    public async Task<List<Grade>> GetAllAsync()
    {
        return await _context.Grades.ToListAsync(); // add as no tracking maybe
    }

    public async Task<Grade> AddAsync(Grade grade)
    {
        _context.Subjects.Attach(grade.Subject);
        _context.Weights.Attach(grade.Weight);

        await _context.Grades.AddAsync(grade);

        await _context.SaveChangesAsync();

        return grade;
    }

    public void UpdateAsync(int id, Grade grade)
    {
        _context.Subjects.Attach(grade.Subject);
        _context.Weights.Attach(grade.Weight);

        _context.Grades.Update(grade);

        _context.SaveChangesAsync();
    }

    public void DeleteByIdAsync(int id)
    {
        var existingGrade = GetByIdAsync(id);

        if (existingGrade != null)
        {
            _context.Grades.Remove(existingGrade.Result);
            _context.SaveChanges();
        }
    }

    public async Task<List<Grade>> GetBySubjectIdsAsync(List<int> ids)
    {
        return await _context.Grades.Where(g => ids.Contains(g.Subject.Id)).Include(g => g.Weight).ToListAsync();
    }

    public async Task<List<Grade>> GetBySubjectIdAsync(int id)
    {
        return await _context.Grades.Where(g => g.Subject.Id == id).Include(g => g.Weight).ToListAsync();
    }

    public async Task<Grade?> GetByIdDetailAsync(int id)
    {
        return await _context.Grades
            .Include(g => g.Weight)
            .Include(g => g.Subject)
            .ThenInclude(s => s.Education)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<List<Grade>> GetAllWithWeightAsync()
    {
        return await _context.Grades.Include(g => g.Weight).ToListAsync();
    }

    public async Task<List<Grade>> GetBySearchWithLimitAsync(string searchValue, int startIndex, int amount)
    {
        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            return await _context.Grades
                .Where(grade =>
                    EF.Functions.Like(grade.Subject.Name.ToLower(), $"%{searchValue}%") ||
                    (grade.Description != null && EF.Functions.Like(grade.Description.ToLower(), $"%{searchValue}%")) ||
                    EF.Functions.Like(grade.Value.ToString(), $"%{searchValue}%") ||
                    EF.Functions.Like(grade.Subject.Education.Name.ToLower(), $"%{searchValue}%") ||
                    EF.Functions.Like(grade.Date.ToString(), $"%{searchValue}%"))
                .Include(g => g.Subject)
                    .ThenInclude(s => s.Education)
                .OrderByDescending(g => g.Date)
                    .ThenByDescending(g => g.Id)
                .Skip(startIndex)
                .Take(amount)
                .ToListAsync();
        }

        return await _context.Grades
            .Include(g => g.Subject)
                .ThenInclude(s => s.Education)
            .OrderByDescending(g => g.Date)
                .ThenByDescending(g => g.Id)
            .Skip(startIndex)
            .Take(amount)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string searchValue)
    {
        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            return await _context.Grades
                .Where(grade =>
                    EF.Functions.Like(grade.Subject.Name.ToLower(), $"%{searchValue}%") ||
                    (grade.Description != null && EF.Functions.Like(grade.Description.ToLower(), $"%{searchValue}%")) ||
                    EF.Functions.Like(grade.Value.ToString(), $"%{searchValue}%") ||
                    EF.Functions.Like(grade.Subject.Education.Name.ToLower(), $"%{searchValue}%") ||
                    EF.Functions.Like(grade.Date.ToString(), $"%{searchValue}%"))
                .CountAsync();
        }

        return await _context.Subjects.CountAsync();
    }
}