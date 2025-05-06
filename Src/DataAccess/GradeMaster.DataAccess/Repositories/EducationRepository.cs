using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GradeMaster.DataAccess.Repositories;

public class EducationRepository : IEducationRepository
{
    private readonly GradeMasterDbContext _context;

    public EducationRepository(GradeMasterDbContext context)
    {
        _context = context;
    }

    public async Task<Education?> GetByIdAsync(int id)
    {
        return await _context.Educations.FindAsync(id);
    }

    public async Task<List<Education>> GetAllAsync()
    { 
        return await _context.Educations.Include(e => e.Subjects).ToListAsync();
    } 

    public async Task<Education> AddAsync(Education education)
    { 
        _context.Subjects.AttachRange(education.Subjects);

        await _context.Educations.AddAsync(education);

        await _context.SaveChangesAsync();

        return education;
    }

    public async Task UpdateAsync(int id, Education education)
    {
        // handle if education doesnt exist in db
        _context.Subjects.AttachRange(education.Subjects);

        _context.Educations.Update(education);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var existingEducation = await GetByIdAsync(id);

        if (existingEducation != null)
        {
            _context.Educations.Remove(existingEducation);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Education?> GetBySubjectIdAsync(int subjectId)
    {
        return await _context.Educations.Where(e => e.Subjects.Any(s => s.Id == subjectId)) // Filter by Subject ID
            .FirstOrDefaultAsync(); // Get the first matching Education, or null if none found;
    }
    // Lambda expression
    //public void DeleteByIdAsync(int id) => throw new NotImplementedException();

    public Task<List<Education>> GetByCompletedAsync(bool completed)
    {
        return _context.Educations.Where(e => e.Completed == completed).ToListAsync();
    }

    // later add order type enum
    public async Task<List<Education>> GetBySearchWithRangeAsync(string searchValue, int startIndex, int amount)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            return await _context.Educations
                .Include(e => e.Subjects)
                    .ThenInclude(s => s.Grades)
                .OrderByDescending(e => e.Id)
                .Skip(startIndex)
                .Take(amount)
                .ToListAsync();
        }

        searchValue = searchValue.Trim();
        var newSearchValue = $"%{searchValue}%";
        var isNumericSearch = int.TryParse(searchValue, out var searchValueAsInt);

        var completionState = searchValue.ToLower();
        bool? searchCompletionState = completionState switch
        {
            "in progress" => false,
            "completed" => true,
            _ => null
        };

        return await _context.Educations
            .Where(education =>
                EF.Functions.Like(education.Name, newSearchValue) ||
                (education.Description != null && EF.Functions.Like(education.Description, newSearchValue)) ||
                (education.Institution != null && EF.Functions.Like(education.Institution, newSearchValue)) ||
                (searchCompletionState != null && education.Completed == searchCompletionState) ||
                (isNumericSearch && education.Semesters == searchValueAsInt) || // Direct integer comparison
                (isNumericSearch && education.StartDate.Year == searchValueAsInt) || // Compare Year as an int
                (isNumericSearch && education.EndDate.Year == searchValueAsInt)
            )
            .Include(e => e.Subjects)
                .ThenInclude(s => s.Grades)
            .OrderByDescending(e => e.Id)
            .Skip(startIndex)
            .Take(amount)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string searchValue)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            return await _context.Educations.CountAsync();
        }

        searchValue = searchValue.Trim();
        var newSearchValue = $"%{searchValue}%";
        var isNumericSearch = int.TryParse(searchValue, out var searchValueAsInt);

        var completionState = searchValue.ToLower();
        bool? searchCompletionState = completionState switch
        {
            "in progress" => false,
            "completed" => true,
            _ => null
        };

        return await _context.Educations
            .Where(education =>
                EF.Functions.Like(education.Name, newSearchValue) ||
                (education.Description != null && EF.Functions.Like(education.Description, newSearchValue)) ||
                (education.Institution != null && EF.Functions.Like(education.Institution, newSearchValue)) ||
                (searchCompletionState != null && education.Completed == searchCompletionState) ||
                (isNumericSearch && education.Semesters == searchValueAsInt) || // Direct integer comparison
                (isNumericSearch && education.StartDate.Year == searchValueAsInt) || // Compare Year as an int
                (isNumericSearch && education.EndDate.Year == searchValueAsInt)
            )
            .CountAsync();
    }

    public async Task<List<Education>> GetAllSimpleAsync()
    {
        return await _context.Educations.OrderByDescending(e => e.Id).ToListAsync(); // maybe change ordering
    }

    public async Task<bool> ExistsAnyAsync()
    {
        return await _context.Educations.AnyAsync();
    }

    public async Task<bool> ExistsAnyIsCompletedAsync(bool completed)
    {
        return await _context.Educations.AnyAsync(e => e.Completed == completed);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Educations.AnyAsync(e => e.Id == id);
    }
}