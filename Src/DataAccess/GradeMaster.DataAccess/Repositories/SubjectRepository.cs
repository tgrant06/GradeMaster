using GradeMaster.DataAccess.Interfaces.IRepositories;
using GradeMaster.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace GradeMaster.DataAccess.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly GradeMasterDbContext _context;

    private static readonly Regex SearchPatternSubjectAndSemester = new(@"^(.*?)(?:\s*-\s*|\s+)(\d+)$", RegexOptions.Compiled);

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

    public async Task<List<Subject>> GetByEducationIdAsync(int educationId)
    {
        return await _context.Subjects.Where(s => s.Education.Id == educationId)
            .Include(s => s.Grades)
            .ToListAsync();
    }

    public async Task<List<Subject>> GetByEducationIdOrderedAsync(int id)
    {
        return await _context.Subjects.Where(s => s.Education.Id == id)
            .Include(s => s.Grades)
            .OrderByDescending(s => s.Semester)
                .ThenByDescending(s => s.Id)
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

    public async Task<List<Subject>> GetByEducationIdAndCompletedAsync(int educationId, bool completed)
    {
        return await _context.Subjects.Where(s => s.Education.Id == educationId && s.Completed == completed)
            .Include(s => s.Education)
            .ToListAsync();
    }

    public async Task<List<Subject>> GetByEducationIdAndCompletedWithSemesterAsync(int educationId, bool completed, int semester)
    {
        return await _context.Subjects.Where(s => s.Education.Id == educationId && s.Completed == completed && s.Semester == semester)
            .Include(s => s.Education)
            .ToListAsync();
    }

    public async Task<Subject?> GetByGradeIdAsync(int gradeId)
    {
        return await _context.Subjects.Where(s => s.Grades.Any(g => g.Id == gradeId)).Include(s => s.Education)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Subject>> GetBySearchWithRangeAsync(string searchValue, int startIndex, int amount)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            return await _context.Subjects
                .Include(s => s.Education)
                .Include(s => s.Grades)
                .OrderByDescending(s => s.Id)
                //.ThenByDescending(s => s.Semester)
                .Skip(startIndex)
                .Take(amount)
                .ToListAsync();
        }

        var mainSearchValue = searchValue.Trim();
        string? institutionSearch = null;

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

        var newSearchValue = $"%{mainSearchValue}%";
        var newInstitutionSearch = !string.IsNullOrEmpty(institutionSearch) ? $"%{institutionSearch}%" : null;


        string? namePart = null;
        int? semesterPart = null;

        var match = SearchPatternSubjectAndSemester.Match(mainSearchValue);
        if (match.Success)
        {
            namePart = match.Groups[1].Value.Trim();
            if (int.TryParse(match.Groups[2].Value, out var sem))
            {
                semesterPart = sem;
            }
        }

        var isNumericSearch = int.TryParse(mainSearchValue, out var searchValueAsInt);

        var completionState = mainSearchValue.ToLower();
        bool? searchCompletionState = completionState switch
        {
            "in progress" => false,
            "completed" => true,
            _ => null
        };

        return await _context.Subjects
            .Where(subject =>
                (
                    (!string.IsNullOrEmpty(namePart) && semesterPart != null
                        ? EF.Functions.Like(subject.Name, $"%{namePart}%") && subject.Semester == semesterPart
                        : EF.Functions.Like(subject.Name, newSearchValue)) ||
                    (subject.Description != null && EF.Functions.Like(subject.Description, newSearchValue)) ||
                    (isNumericSearch && subject.Semester == searchValueAsInt) ||
                    (searchCompletionState != null && subject.Completed == searchCompletionState) ||
                    EF.Functions.Like(subject.Education.Name, newSearchValue) ||
                    (newInstitutionSearch == null &&
                     subject.Education.Institution != null && EF.Functions.Like(subject.Education.Institution, newSearchValue))
                )
                &&
                (
                    newInstitutionSearch == null ||
                    (subject.Education.Institution != null && EF.Functions.Like(subject.Education.Institution, newInstitutionSearch))
                )
            )
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
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            return await _context.Subjects.CountAsync();
        }

        var mainSearchValue = searchValue.Trim();
        string? institutionSearch = null;

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

        var newSearchValue = $"%{mainSearchValue}%";
        var newInstitutionSearch = !string.IsNullOrEmpty(institutionSearch) ? $"%{institutionSearch}%" : null;


        string? namePart = null;
        int? semesterPart = null;

        var match = SearchPatternSubjectAndSemester.Match(mainSearchValue);
        if (match.Success)
        {
            namePart = match.Groups[1].Value.Trim();
            if (int.TryParse(match.Groups[2].Value, out var sem))
            {
                semesterPart = sem;
            }
        }

        var isNumericSearch = int.TryParse(mainSearchValue, out var searchValueAsInt);

        var completionState = mainSearchValue.ToLower();
        bool? searchCompletionState = completionState switch
        {
            "in progress" => false,
            "completed" => true,
            _ => null
        };

        return await _context.Subjects
            .Where(subject =>
                (
                    (!string.IsNullOrEmpty(namePart) && semesterPart != null
                        ? EF.Functions.Like(subject.Name, $"%{namePart}%") && subject.Semester == semesterPart
                        : EF.Functions.Like(subject.Name, newSearchValue)) ||
                    (subject.Description != null && EF.Functions.Like(subject.Description, newSearchValue)) ||
                    (isNumericSearch && subject.Semester == searchValueAsInt) ||
                    (searchCompletionState != null && subject.Completed == searchCompletionState) ||
                    EF.Functions.Like(subject.Education.Name, newSearchValue) ||
                    (newInstitutionSearch == null &&
                     subject.Education.Institution != null && EF.Functions.Like(subject.Education.Institution, newSearchValue))
                )
                &&
                (
                    newInstitutionSearch == null ||
                    (subject.Education.Institution != null && EF.Functions.Like(subject.Education.Institution, newInstitutionSearch))
                )
            )
            .CountAsync();
    }

    public async Task<bool> ExistsAnyAsync()
    {
        return await _context.Subjects.AnyAsync();
    }

    public async Task<bool> ExistsAnyIsCompletedAsync(bool completed)
    {
        return await _context.Subjects.AnyAsync(s => s.Completed == completed);
    }

    public async Task<bool> ExistsAnyIsCompletedWithEducationIdAsync(int educationId, bool completed)
    {
        return await _context.Subjects.AnyAsync(s => s.Education.Id == educationId && s.Completed == completed);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Subjects.AnyAsync(s => s.Id == id);
    }
}