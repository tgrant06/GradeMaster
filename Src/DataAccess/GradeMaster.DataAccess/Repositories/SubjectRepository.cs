using GradeMaster.DataAccess.Interfaces.IRepositories;
using GradeMaster.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace GradeMaster.DataAccess.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly GradeMasterDbContext _context;

    public SubjectRepository(GradeMasterDbContext context)
    {
        _context = context;
    }


    public async Task<Subject?> GetByIdAsync(int id)
    {
        return await _context.Subjects.FindAsync(id);
    }

    public async Task<List<Subject>> GetAllAsync()
    {
        return await _context.Subjects.Include(s => s.Education).ToListAsync();
    }

    public async Task<Subject> AddAsync(Subject subject)
    {
        // maybe include if statement
        _context.Educations.Attach(subject.Education);
        _context.Grades.AttachRange(subject.Grades);

        await _context.Subjects.AddAsync(subject);

        await _context.SaveChangesAsync();

        return subject;
    }

    public async Task UpdateAsync(int id, Subject subject)
    {
        _context.Educations.Attach(subject.Education);
        _context.Grades.AttachRange(subject.Grades);

        _context.Subjects.Update(subject);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var existingSubject = await GetByIdAsync(id);

        if (existingSubject != null)
        {
            // the deletion of the related entities is handled by the database
            _context.Subjects.Remove(existingSubject);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Subject>> GetByEducationIdAsync(int id)
    {
        return await _context.Subjects.Where(s => s.Education.Id == id)
            .Include(s => s.Grades)
            //.ThenInclude(g => g.Weight)
            .ToListAsync();
    }

    public async Task<List<Subject>> GetAllWithGradesAsync()
    {
        return await _context.Subjects.Where(s => s.Grades.Any()).Include(s => s.Education).ToListAsync();
    }

    public async Task<Subject?> GetByIdDetailAsync(int id)
    {
        return await _context.Subjects.Include(s => s.Education).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Subject>> GetByCompletedAsync(bool completed)
    {
        return await _context.Subjects.Where(s => s.Completed == completed).Include(s => s.Education).ToListAsync();
    }

    public async Task<Subject?> GetByGradeIdAsync(int id)
    {
        return await _context.Subjects.Where(s => s.Grades.Any(g => g.Id == id)).Include(s => s.Education)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Subject>> GetBySearchWithLimitAsync(string searchValue, int startIndex, int amount)
    {
        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            var newSearchValue = $"%{searchValue}%";

            bool isNumericSearch = int.TryParse(searchValue, out int searchValueAsInt);

            return await _context.Subjects
                .Where(subject =>
                    EF.Functions.Like(subject.Name, newSearchValue) ||
                    (subject.Description != null && EF.Functions.Like(subject.Description, newSearchValue)) ||
                    (isNumericSearch && subject.Semester == searchValueAsInt) ||
                    EF.Functions.Like(subject.Education.Name, newSearchValue) ||
                    (subject.Education.Institution != null && EF.Functions.Like(subject.Education.Institution, newSearchValue)))
                .Include(s => s.Education)
                .Include(s => s.Grades)
                .OrderByDescending(s => s.Id)
                    //.ThenByDescending(s => s.Semester)
                .Skip(startIndex)
                .Take(amount)
                .ToListAsync();
        }

        return await _context.Subjects
            .Include(s => s.Education)
            .Include(s => s.Grades)
            .OrderByDescending(s => s.Id)
                //.ThenByDescending(s => s.Semester)
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

            return await _context.Subjects
                .Where(subject =>
                    EF.Functions.Like(subject.Name, newSearchValue) ||
                    (subject.Description != null && EF.Functions.Like(subject.Description, newSearchValue)) ||
                    (isNumericSearch && subject.Semester == searchValueAsInt) ||
                    EF.Functions.Like(subject.Education.Name, newSearchValue) ||
                    (subject.Education.Institution != null && EF.Functions.Like(subject.Education.Institution, newSearchValue)))
                .CountAsync();
        }

        return await _context.Subjects.CountAsync();
    }

    public async Task<bool> ExistsAnyAsync()
    {
        return await _context.Subjects.AnyAsync();
    }

    public async Task<bool> ExistsAnyIsCompletedAsync(bool completed)
    {
        return await _context.Subjects.AnyAsync(s => s.Completed == completed);
    }
}