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

    public async Task UpdateAsync(int id, Grade grade)
    {
        _context.Subjects.Attach(grade.Subject);
        _context.Weights.Attach(grade.Weight);

        _context.Grades.Update(grade);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var existingGrade = await GetByIdAsync(id);

        if (existingGrade != null)
        {
            _context.Grades.Remove(existingGrade);
            await _context.SaveChangesAsync();
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
            var newSearchValue = $"%{searchValue}%";

            bool isNumericSearch = decimal.TryParse(searchValue, out decimal searchValueAsDecimal);

            return await _context.Grades
                // maybe add search by weight
                // maybe add search by Subject.Semester
                .Where(grade =>
                    EF.Functions.Like(grade.Subject.Name, newSearchValue) ||
                    (grade.Description != null && EF.Functions.Like(grade.Description, newSearchValue)) ||
                    (isNumericSearch && grade.Value == searchValueAsDecimal) ||
                    EF.Functions.Like(grade.Subject.Education.Name, newSearchValue) ||
                    EF.Functions.Like(grade.Date.ToString(), newSearchValue) ||
                    (grade.Subject.Education.Institution != null && EF.Functions.Like(grade.Subject.Education.Institution, newSearchValue)))
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
            var newSearchValue = $"%{searchValue}%";

            bool isNumericSearch = decimal.TryParse(searchValue, out decimal searchValueAsDecimal);

            return await _context.Grades
                .Where(grade =>
                    EF.Functions.Like(grade.Subject.Name, newSearchValue) ||
                    (grade.Description != null && EF.Functions.Like(grade.Description, newSearchValue)) ||
                    (isNumericSearch && grade.Value == searchValueAsDecimal) ||
                    EF.Functions.Like(grade.Subject.Education.Name, newSearchValue) ||
                    EF.Functions.Like(grade.Date.ToString(), newSearchValue) ||
                    (grade.Subject.Education.Institution != null && EF.Functions.Like(grade.Subject.Education.Institution, newSearchValue)))
                .CountAsync();
        }

        return await _context.Grades.CountAsync();
    }

    public async Task<bool> ExistsAnyAsync()
    {
        return await _context.Grades.AnyAsync();
    }
}