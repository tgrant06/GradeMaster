using System.Text.RegularExpressions;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GradeMaster.DataAccess.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly GradeMasterDbContext _context;

    private static readonly Regex DateRegex = new(@"^(\d{1,2})\.(\d{1,2})\.(\d{4})$", RegexOptions.Compiled);
    private static readonly Regex MonthYearRegex = new(@"^(\d{1,2})\.(\d{4})$", RegexOptions.Compiled);

    public NoteRepository(GradeMasterDbContext context)
    {
        _context = context;
    }

    // TODO: update for DateTime format
    private static string NormalizeDateSearchValue(string searchValue)
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

        var matchMonthYear = MonthYearRegex.Match(searchValue);
        if (matchMonthYear.Success)
        {
            // Extract day, month, and year
            var month = matchMonthYear.Groups[1].Value.PadLeft(2, '0');
            var year = matchMonthYear.Groups[2].Value;

            // Return in yyyy-mm format
            return $"{year}-{month}";
        }

        return searchValue;
    }

    public async Task<Note?> GetByIdAsync(int id)
    {
        return await _context.Notes.FindAsync(id);
    }

    public async Task<List<Note>> GetAllAsync()
    {
        return await _context.Notes.ToListAsync();
    }

    public async Task<Note> AddAsync(Note note)
    {
        _context.Colors.Attach(note.Color);

        await _context.Notes.AddAsync(note);

        await _context.SaveChangesAsync();

        return note;
    }

    public async Task UpdateAsync(int id, Note note)
    {
        _context.Colors.Attach(note.Color);

        _context.Notes.Update(note);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var existingNote = await GetByIdAsync(id);

        if (existingNote != null)
        {
            _context.Notes.Remove(existingNote);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Note>> GetBySearchWithRangeAsync(string searchValue, int startIndex, int amount)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            return await _context.Notes
                .OrderByDescending(n => n.IsPinned)
                    .ThenByDescending(n => n.UpdatedAt)
                    .ThenByDescending(n => n.Id)
                .Skip(startIndex)
                .Take(amount)
                .ToListAsync();
        }

        var mainSearchValue = searchValue.Trim();
        var searchForOnlyIsArchived = false;
        var searchForOnlyIsPinned = false;

        // Check for completion state suffix
        if (mainSearchValue.EndsWith(" (archive)", StringComparison.OrdinalIgnoreCase))
        {
            searchForOnlyIsArchived = true;
            mainSearchValue = mainSearchValue[..^" (archive)".Length].Trim();
        }
        else if (mainSearchValue.EndsWith(" (a)", StringComparison.OrdinalIgnoreCase))
        {
            searchForOnlyIsArchived = true;
            mainSearchValue = mainSearchValue[..^" (a)".Length].Trim();
        }
        else if (mainSearchValue.EndsWith(" (pinned)", StringComparison.OrdinalIgnoreCase))
        {
            searchForOnlyIsArchived = true;
            mainSearchValue = mainSearchValue[..^" (pinned)".Length].Trim();
        }
        else if (mainSearchValue.EndsWith(" (p)", StringComparison.OrdinalIgnoreCase))
        {
            searchForOnlyIsArchived = true;
            mainSearchValue = mainSearchValue[..^" (p)".Length].Trim();
        }

        string? colorSearch = null;

        // Check for pipe separator
        if (mainSearchValue.Contains(" | "))
        {
            var parts = mainSearchValue.Split('|', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                mainSearchValue = parts[0].Trim();
                colorSearch = parts[1].Trim();
            }
        }

        // If after removing suffixes and splitting, mainSearchValue is empty, we'll only search by completion state
        var searchByTextValue = !string.IsNullOrWhiteSpace(mainSearchValue);

        var newSearchValue = searchByTextValue ? $"%{mainSearchValue}%" : null;
        var newColorSearch = !string.IsNullOrEmpty(colorSearch) ? $"%{colorSearch}%" : null;

        // Only check for in progress/completed in the search value if we haven't already found a suffix
        if (searchForOnlyIsArchived == false && searchByTextValue)
        {
            var completionState = mainSearchValue.ToLower();
            searchForOnlyIsArchived = completionState switch
            {
                "*(archive)" => true,
                "*(a)" => true,
                _ => false
            };
        }

        if (searchForOnlyIsPinned == false && searchByTextValue)
        {
            var completionState = mainSearchValue.ToLower();
            searchForOnlyIsPinned = completionState switch
            {
                "*(pinned)" => true,
                "*(p)" => true,
                _ => false
            };
        }

        var normalizedDateSearch = NormalizeDateSearchValue(mainSearchValue);
        var normalizedDateSearchValue = $"%{normalizedDateSearch}%";

        return await _context.Notes
            .Where(note =>
                (
                    !searchByTextValue ||
                    EF.Functions.Like(note.Title, newSearchValue) ||
                    (note.Content != null && EF.Functions.Like(note.Content, newSearchValue)) ||
                    EF.Functions.Like(note.UpdatedAt.ToString(), normalizedDateSearchValue) ||
                    EF.Functions.Like(note.CreatedAt.ToString(), normalizedDateSearchValue) ||
                    (newColorSearch == null &&
                     EF.Functions.Like(note.Color.Name + note.Color.Symbol, newSearchValue))
                )
                &&
                (
                    newColorSearch == null ||
                    EF.Functions.Like(note.Color.Name + note.Color.Symbol, newColorSearch)
                )
                &&
                (
                    !searchForOnlyIsPinned ||
                    note.IsPinned == searchForOnlyIsPinned
                )
                && 
                    note.IsArchived == searchForOnlyIsArchived
            )
            .OrderByDescending(n => n.IsPinned)
                .ThenByDescending(n => n.UpdatedAt)
                .ThenByDescending(n => n.Id)
            .Skip(startIndex)
            .Take(amount)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string searchValue)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            return await _context.Notes.CountAsync();
        }

        var mainSearchValue = searchValue.Trim();
        var searchForOnlyIsArchived = false;
        var searchForOnlyIsPinned = false;

        // Check for completion state suffix
        if (mainSearchValue.EndsWith(" (archive)", StringComparison.OrdinalIgnoreCase))
        {
            searchForOnlyIsArchived = true;
            mainSearchValue = mainSearchValue[..^" (archive)".Length].Trim();
        }
        else if (mainSearchValue.EndsWith(" (a)", StringComparison.OrdinalIgnoreCase))
        {
            searchForOnlyIsArchived = true;
            mainSearchValue = mainSearchValue[..^" (a)".Length].Trim();
        }
        else if (mainSearchValue.EndsWith(" (pinned)", StringComparison.OrdinalIgnoreCase))
        {
            searchForOnlyIsArchived = true;
            mainSearchValue = mainSearchValue[..^" (pinned)".Length].Trim();
        }
        else if (mainSearchValue.EndsWith(" (p)", StringComparison.OrdinalIgnoreCase))
        {
            searchForOnlyIsArchived = true;
            mainSearchValue = mainSearchValue[..^" (p)".Length].Trim();
        }

        string? colorSearch = null;

        // Check for pipe separator
        if (mainSearchValue.Contains(" | "))
        {
            var parts = mainSearchValue.Split('|', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                mainSearchValue = parts[0].Trim();
                colorSearch = parts[1].Trim();
            }
        }

        // If after removing suffixes and splitting, mainSearchValue is empty, we'll only search by completion state
        var searchByTextValue = !string.IsNullOrWhiteSpace(mainSearchValue);

        var newSearchValue = searchByTextValue ? $"%{mainSearchValue}%" : null;
        var newColorSearch = !string.IsNullOrEmpty(colorSearch) ? $"%{colorSearch}%" : null;

        // Only check for in progress/completed in the search value if we haven't already found a suffix
        if (searchForOnlyIsArchived == false && searchByTextValue)
        {
            var completionState = mainSearchValue.ToLower();
            searchForOnlyIsArchived = completionState switch
            {
                "*(archive)" => true,
                "*(a)" => true,
                _ => false
            };
        }

        if (searchForOnlyIsPinned == false && searchByTextValue)
        {
            var completionState = mainSearchValue.ToLower();
            searchForOnlyIsPinned = completionState switch
            {
                "*(pinned)" => true,
                "*(p)" => true,
                _ => false
            };
        }

        var normalizedDateSearch = NormalizeDateSearchValue(mainSearchValue);
        var normalizedDateSearchValue = $"%{normalizedDateSearch}%";

        return await _context.Notes
            .Where(note =>
                (
                    !searchByTextValue ||
                    EF.Functions.Like(note.Title, newSearchValue) ||
                    (note.Content != null && EF.Functions.Like(note.Content, newSearchValue)) ||
                    EF.Functions.Like(note.UpdatedAt.ToString(), normalizedDateSearchValue) ||
                    EF.Functions.Like(note.CreatedAt.ToString(), normalizedDateSearchValue) ||
                    (newColorSearch == null &&
                     EF.Functions.Like(note.Color.Name + note.Color.Symbol, newSearchValue))
                )
                &&
                (
                    newColorSearch == null ||
                    EF.Functions.Like(note.Color.Name + note.Color.Symbol, newColorSearch)
                )
                &&
                (
                    !searchForOnlyIsPinned ||
                    note.IsPinned == searchForOnlyIsPinned
                )
                &&
                    note.IsArchived == searchForOnlyIsArchived
            )
            .CountAsync();
    }

    public async Task<bool> ExistsAnyAsync()
    {
        return await _context.Notes.AnyAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Notes.AnyAsync(n => n.Id == id);
    }
}