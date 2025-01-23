using SQLitePCL;
using GradeMaster.Shared.Core.Entities;
using GradeMaster.Shared.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Buffers;

namespace GradeMaster.DataAccess.Core.Repositories;

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

    public void UpdateAsync(int id, Education education)
    {
        // handle if education doesnt exist in db
        _context.Subjects.AttachRange(education.Subjects);

        _context.Educations.Update(education);

        _context.SaveChangesAsync();
    }

    public void DeleteByIdAsync(int id)
    {
        var existingBlog = GetByIdAsync(id);

        if (existingBlog != null)
        {
            _context.Educations.Remove(existingBlog.Result);
            _context.SaveChangesAsync();
        }
    }

    public Task<List<Education>> GetBySubjectIdsAsync(List<int> ids)
    {
        return null;
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
            return await _context.Educations
                //.Where(education =>
                //    education.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                //    (education.Description != null && education.Description.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                //    education.Semesters.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                //    (education.Institution != null && education.Institution.Contains(searchValue, StringComparison.OrdinalIgnoreCase)))
                .Where(education =>
                    EF.Functions.Like(education.Name.ToLower(), $"%{searchValue}%") ||
                    (education.Description != null && EF.Functions.Like(education.Description.ToLower(), $"%{searchValue}%")) ||
                    EF.Functions.Like(education.Semesters.ToString().ToLower(), $"%{searchValue}%") ||
                    (education.Institution != null && EF.Functions.Like(education.Institution.ToLower(), $"%{searchValue}%")))
                .Include(e => e.Subjects).ThenInclude(s => s.Grades)
                .OrderByDescending(e => e.Id)
                .Skip(startIndex)
                .Take(amount)
                .ToListAsync();
        }

        return await _context.Educations
            .Include(e => e.Subjects).ThenInclude(s => s.Grades)
            .OrderByDescending(e => e.Id)
            .Skip(startIndex)
            .Take(amount)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string searchValue)
    {
        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            return await _context.Educations
                .Where(education =>
                    EF.Functions.Like(education.Name.ToLower(), $"%{searchValue}%") ||
                    (education.Description != null && EF.Functions.Like(education.Description.ToLower(), $"%{searchValue}%")) ||
                    EF.Functions.Like(education.Semesters.ToString().ToLower(), $"%{searchValue}%") ||
                    (education.Institution != null && EF.Functions.Like(education.Institution.ToLower(), $"%{searchValue}%")))
                .CountAsync();
        }

        return await _context.Educations.CountAsync();
    }

    public async Task<List<Education>> GetAllSimpleAsync()
    {
        return await _context.Educations.ToListAsync(); // maybe change ordering
    }
}