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
    public async Task<List<Education>> GetBySearchWithLimitAsync(string searchValue, int startIndex, int amount)
    {
        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            var newSearchValue = $"%{searchValue}%";

            bool isNumericSearch = int.TryParse(searchValue, out int searchValueAsInt);

            return await _context.Educations
                //.Where(education =>
                //    education.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                //    (education.Description != null && education.Description.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                //    education.Semesters.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                //    (education.Institution != null && education.Institution.Contains(searchValue, StringComparison.OrdinalIgnoreCase)))
                //.Where(education =>
                //    EF.Functions.Like(education.Name, newSearchValue) ||
                //    (education.Description != null && EF.Functions.Like(education.Description, newSearchValue)) ||
                //    EF.Functions.Like(education.Semesters.ToString(), newSearchValue) ||
                //    (education.Institution != null && EF.Functions.Like(education.Institution, newSearchValue)) ||
                //    EF.Functions.Like(education.StartDate.Year.ToString(), newSearchValue) || // Search in StartDate
                //    EF.Functions.Like(education.EndDate.Year.ToString(), newSearchValue))
                .Where(education =>
                    EF.Functions.Like(education.Name, newSearchValue) ||
                    (education.Description != null && EF.Functions.Like(education.Description, newSearchValue)) ||
                    (education.Institution != null && EF.Functions.Like(education.Institution, newSearchValue)) ||
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

        return await _context.Educations
            .Include(e => e.Subjects)
                .ThenInclude(s => s.Grades)
            .OrderByDescending(e => e.Id)
            .Skip(startIndex)
            .Take(amount)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string searchValue)
    {
        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            var newSearchValue = $"%{searchValue}%";

            bool isNumericSearch = int.TryParse(searchValue, out int searchValueAsInt);

            return await _context.Educations
                .Where(education =>
                    EF.Functions.Like(education.Name, newSearchValue) ||
                    (education.Description != null && EF.Functions.Like(education.Description, newSearchValue)) ||
                    (education.Institution != null && EF.Functions.Like(education.Institution, newSearchValue)) ||
                    (isNumericSearch && education.Semesters == searchValueAsInt) || // Direct integer comparison
                    (isNumericSearch && education.StartDate.Year == searchValueAsInt) || // Compare Year as an int
                    (isNumericSearch && education.EndDate.Year == searchValueAsInt)
                )
                .CountAsync();
        }

        return await _context.Educations.CountAsync();
    }

    public async Task<List<Education>> GetAllSimpleAsync()
    {
        return await _context.Educations.ToListAsync(); // maybe change ordering
    }

    public async Task<bool> ExistsAnyAsync()
    {
        return await _context.Educations.AnyAsync();
    }

    public async Task<bool> ExistsAnyIsCompletedAsync(bool completed)
    {
        return await _context.Educations.AnyAsync(e => e.Completed == completed);
    }
}