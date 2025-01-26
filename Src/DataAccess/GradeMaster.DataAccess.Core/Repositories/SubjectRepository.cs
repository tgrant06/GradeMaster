using GradeMaster.Shared.Core.Entities;
using GradeMaster.Shared.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GradeMaster.DataAccess.Core.Repositories;

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

    public void UpdateAsync(int id, Subject subject)
    {
        _context.Educations.Attach(subject.Education);
        _context.Grades.AttachRange(subject.Grades);

        _context.Subjects.Update(subject);

        _context.SaveChangesAsync();
    }

    public void DeleteByIdAsync(int id)
    {
        var existingSubject = GetByIdAsync(id);

        if (existingSubject != null)
        {
            // the deletion of the related entities is handled by the database
            _context.Subjects.Remove(existingSubject.Result);
            _context.SaveChanges();
        }
    }

    public async Task<List<Subject>> GetByEducationIdAsync(int id)
    {
        return await _context.Subjects.Where(s => s.Education.Id == id).Include(s => s.Grades)
            .ThenInclude(g => g.Weight).ToListAsync();
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
            return await _context.Subjects
                .Where(subject =>
                    EF.Functions.Like(subject.Name.ToLower(), $"%{searchValue}%") ||
                    (subject.Description != null && EF.Functions.Like(subject.Description.ToLower(), $"%{searchValue}%")) ||
                    EF.Functions.Like(subject.Semester.ToString(), $"%{searchValue}%") ||
                    EF.Functions.Like(subject.Education.Name.ToLower(), $"%{searchValue}%") ||
                    (subject.Education.Institution != null && EF.Functions.Like(subject.Education.Institution.ToLower(), $"%{searchValue}%")))
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
            return await _context.Subjects
                .Where(subject =>
                    EF.Functions.Like(subject.Name.ToLower(), $"%{searchValue}%") ||
                    (subject.Description != null && EF.Functions.Like(subject.Description.ToLower(), $"%{searchValue}%")) ||
                    EF.Functions.Like(subject.Semester.ToString(), $"%{searchValue}%") ||
                    EF.Functions.Like(subject.Education.Name.ToLower(), $"%{searchValue}%") ||
                    (subject.Education.Institution != null && EF.Functions.Like(subject.Education.Institution.ToLower(), $"%{searchValue}%")))
                .CountAsync();
        }

        return await _context.Subjects.CountAsync();
    }
}