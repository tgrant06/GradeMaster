using Microsoft.EntityFrameworkCore;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using System.Text.RegularExpressions;

namespace GradeMaster.DataAccess.Repositories;

public class GradeRepository : IGradeRepository
{
    private readonly GradeMasterDbContext _context;
    private static readonly Regex DateRegex = new(@"^(\d{1,2})\.(\d{1,2})\.(\d{4})$", RegexOptions.Compiled);

    public GradeRepository(GradeMasterDbContext context)
    {
        _context = context;
    }

    private string NormalizeDateSearchValue(string searchValue)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
            return searchValue;

        // Check if the input matches the dd.mm.yyyy pattern
        var match = DateRegex.Match(searchValue);
        if (match.Success)
        {
            // Extract day, month, and year
            var day = match.Groups[1].Value.PadLeft(2, '0');
            var month = match.Groups[2].Value.PadLeft(2, '0');
            var year = match.Groups[3].Value;

            // Return in yyyy-mm-dd format
            return $"{year}-{month}-{day}";
        }

        return searchValue;
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
        return await _context.Grades
            .Where(g => g.Subject.Id == id)
            .OrderByDescending(g => g.Date)
                .ThenByDescending(g => g.Id)
            .ToListAsync();
    }

    public async Task<List<Grade>> GetBySubjectIdSimpleAsync(int id)
    {
        return await _context.Grades.Where(g => g.Subject.Id == id).ToListAsync();
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

    public async Task<List<Grade>> GetBySearchWithRangeAsync(string searchValue, int startIndex, int amount)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            return await _context.Grades
                .Include(g => g.Subject)
                    .ThenInclude(s => s.Education)
                .OrderByDescending(g => g.Date)
                    .ThenByDescending(g => g.Id)
                .Skip(startIndex)
                .Take(amount)
                .ToListAsync();
        }

        searchValue = searchValue.Trim();
        var newSearchValue = $"%{searchValue}%";
        var normalizedDateSearch = NormalizeDateSearchValue(searchValue);
        var normalizedDateSearchValue = $"%{normalizedDateSearch}%";
        var isNumericSearch = decimal.TryParse(searchValue, out var searchValueAsDecimal);

        return await _context.Grades
            // maybe add search by weight
            // maybe add search by Subject.Semester
            .Where(grade =>
                EF.Functions.Like(grade.Subject.Name, newSearchValue) ||
                (grade.Description != null && EF.Functions.Like(grade.Description, newSearchValue)) ||
                (isNumericSearch && grade.Value == searchValueAsDecimal) ||
                (isNumericSearch && grade.Weight.Value == searchValueAsDecimal) ||
                EF.Functions.Like(grade.Weight.Name, newSearchValue) ||
                EF.Functions.Like(grade.Subject.Education.Name, newSearchValue) ||
                EF.Functions.Like(grade.Date.ToString(), normalizedDateSearchValue) ||
                (grade.Subject.Education.Institution != null && EF.Functions.Like(grade.Subject.Education.Institution, newSearchValue)))
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
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            return await _context.Grades.CountAsync();
        }

        searchValue = searchValue.Trim();
        var newSearchValue = $"%{searchValue}%";
        var normalizedDateSearch = NormalizeDateSearchValue(searchValue);
        var normalizedDateSearchValue = $"%{normalizedDateSearch}%";
        var isNumericSearch = decimal.TryParse(searchValue, out var searchValueAsDecimal);

        return await _context.Grades
            .Where(grade =>
                EF.Functions.Like(grade.Subject.Name, newSearchValue) ||
                (grade.Description != null && EF.Functions.Like(grade.Description, newSearchValue)) ||
                (isNumericSearch && grade.Value == searchValueAsDecimal) ||
                (isNumericSearch && grade.Weight.Value == searchValueAsDecimal) ||
                EF.Functions.Like(grade.Weight.Name, newSearchValue) ||
                EF.Functions.Like(grade.Subject.Education.Name, newSearchValue) ||
                EF.Functions.Like(grade.Date.ToString(), normalizedDateSearchValue) ||
                (grade.Subject.Education.Institution != null && EF.Functions.Like(grade.Subject.Education.Institution, newSearchValue)))
            .CountAsync();
    }

    public async Task<bool> ExistsAnyAsync()
    {
        return await _context.Grades.AnyAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Grades.AnyAsync(g => g.Id == id);
    }
}