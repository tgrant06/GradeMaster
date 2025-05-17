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

        var mainSearchValue = searchValue.Trim();
        string? institutionSearch = null;
        bool? searchCompletionState = null;

        // Check for completion state suffix
        if (mainSearchValue.EndsWith(" (in progress)", StringComparison.OrdinalIgnoreCase))
        {
            searchCompletionState = false;
            mainSearchValue = mainSearchValue[..^" (in progress)".Length].Trim();
        } 
        else if (mainSearchValue.EndsWith(" (ip)", StringComparison.OrdinalIgnoreCase))
        {
            searchCompletionState = false;
            mainSearchValue = mainSearchValue[..^" (ip)".Length].Trim();
        }
        else if (mainSearchValue.EndsWith(" (complete)", StringComparison.OrdinalIgnoreCase))
        {
            searchCompletionState = true;
            mainSearchValue = mainSearchValue[..^" (complete)".Length].Trim();
        } 
        else if (mainSearchValue.EndsWith(" (c)", StringComparison.OrdinalIgnoreCase))
        {
            searchCompletionState = true;
            mainSearchValue = mainSearchValue[..^" (c)".Length].Trim();
        }

        // Check for pipe separator
        if (mainSearchValue.Contains(" | "))
        {
            var parts = mainSearchValue.Split('|', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                mainSearchValue = parts[0].Trim();
                institutionSearch = parts[1].Trim();
            }
        }

        // If after removing suffixes and splitting, mainSearchValue is empty, we'll only search by completion state
        var searchByTextValue = !string.IsNullOrWhiteSpace(mainSearchValue);

        var newSearchValue = searchByTextValue ? $"%{mainSearchValue}%" : null;
        var newInstitutionSearch = !string.IsNullOrEmpty(institutionSearch) ? $"%{institutionSearch}%" : null;
        var isSearchValueNumber = int.TryParse(mainSearchValue, out var searchValueAsInt);
        var isNumericSearch = searchByTextValue && isSearchValueNumber;

        // Only check for in progress/completed in the search value if we haven't already found a suffix
        var onlySearchByCompletionState = false;

        if (searchCompletionState == null && searchByTextValue)
        {
            var completionState = mainSearchValue.ToLower();
            searchCompletionState = completionState switch
            {
                "*(in progress)" => false,
                "*(ip)" => false,
                "*(completed)" => true,
                "*(c)" => true,
                _ => null
            };

            onlySearchByCompletionState = searchCompletionState != null;
        }

        return await _context.Educations
            .Where(education =>
                (
                    // Skip text-based searches if the main search value is empty
                    onlySearchByCompletionState ||
                    !searchByTextValue ||
                    EF.Functions.Like(education.Name, newSearchValue) ||
                    (education.Description != null && EF.Functions.Like(education.Description, newSearchValue)) ||
                    (isNumericSearch && education.Semesters == searchValueAsInt) ||
                    (isNumericSearch && education.StartDate.Year == searchValueAsInt) ||
                    (isNumericSearch && education.EndDate.Year == searchValueAsInt) ||
                    (newInstitutionSearch == null &&
                     education.Institution != null && EF.Functions.Like(education.Institution, newSearchValue))
                )
                &&
                (
                    newInstitutionSearch == null ||
                    (education.Institution != null && EF.Functions.Like(education.Institution, newInstitutionSearch))
                )
                &&
                (
                    searchCompletionState == null ||
                    education.Completed == searchCompletionState
                )
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

        var mainSearchValue = searchValue.Trim();
        string? institutionSearch = null;
        bool? searchCompletionState = null;

        // Check for completion state suffix
        if (mainSearchValue.EndsWith(" (in progress)", StringComparison.OrdinalIgnoreCase))
        {
            searchCompletionState = false;
            mainSearchValue = mainSearchValue[..^" (in progress)".Length].Trim();
        }
        else if (mainSearchValue.EndsWith(" (ip)", StringComparison.OrdinalIgnoreCase))
        {
            searchCompletionState = false;
            mainSearchValue = mainSearchValue[..^" (ip)".Length].Trim();
        }
        else if (mainSearchValue.EndsWith(" (complete)", StringComparison.OrdinalIgnoreCase))
        {
            searchCompletionState = true;
            mainSearchValue = mainSearchValue[..^" (complete)".Length].Trim();
        }
        else if (mainSearchValue.EndsWith(" (c)", StringComparison.OrdinalIgnoreCase))
        {
            searchCompletionState = true;
            mainSearchValue = mainSearchValue[..^" (c)".Length].Trim();
        }

        // Check for pipe separator
        if (mainSearchValue.Contains(" | "))
        {
            var parts = mainSearchValue.Split('|', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                mainSearchValue = parts[0].Trim();
                institutionSearch = parts[1].Trim();
            }
        }

        // If after removing suffixes and splitting, mainSearchValue is empty, we'll only search by completion state
        var searchByTextValue = !string.IsNullOrWhiteSpace(mainSearchValue);

        var newSearchValue = searchByTextValue ? $"%{mainSearchValue}%" : null;
        var newInstitutionSearch = !string.IsNullOrEmpty(institutionSearch) ? $"%{institutionSearch}%" : null;
        var isSearchValueNumber = int.TryParse(mainSearchValue, out var searchValueAsInt);
        var isNumericSearch = searchByTextValue && isSearchValueNumber;

        // Only check for in progress/completed in the search value if we haven't already found a suffix
        var onlySearchByCompletionState = false;

        if (searchCompletionState == null && searchByTextValue)
        {
            var completionState = mainSearchValue.ToLower();
            searchCompletionState = completionState switch
            {
                "*(in progress)" => false,
                "*(ip)" => false,
                "*(completed)" => true,
                "*(c)" => true,
                _ => null
            };

            onlySearchByCompletionState = searchCompletionState != null;
        }

        return await _context.Educations
            .Where(education =>
                (
                    // Skip text-based searches if the main search value is empty
                    onlySearchByCompletionState ||
                    !searchByTextValue ||
                    EF.Functions.Like(education.Name, newSearchValue) ||
                    (education.Description != null && EF.Functions.Like(education.Description, newSearchValue)) ||
                    (isNumericSearch && education.Semesters == searchValueAsInt) ||
                    (isNumericSearch && education.StartDate.Year == searchValueAsInt) ||
                    (isNumericSearch && education.EndDate.Year == searchValueAsInt) ||
                    (newInstitutionSearch == null &&
                     education.Institution != null && EF.Functions.Like(education.Institution, newSearchValue))
                )
                &&
                (
                    newInstitutionSearch == null ||
                    (education.Institution != null && EF.Functions.Like(education.Institution, newInstitutionSearch))
                )
                &&
                (
                    searchCompletionState == null ||
                    education.Completed == searchCompletionState
                )
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