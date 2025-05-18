using GradeMaster.Common.Entities;
using GradeMaster.Logic.Interfaces.IServices;

namespace GradeMaster.Logic.Services;

public class NoteService : INoteService
{
    public void PassObjectAttributes(Note toNote, Note fromNote, bool actionSubmit = false)
    {
        ArgumentNullException.ThrowIfNull(fromNote);

        ArgumentNullException.ThrowIfNull(toNote);

        toNote.Title = actionSubmit ? fromNote.Title.Trim( ) : fromNote.Title;
        toNote.Content  = actionSubmit ? fromNote.Content?.Trim() : fromNote.Content;
        toNote.CreatedAt = fromNote.CreatedAt;
        toNote.UpdatedAt = fromNote.UpdatedAt;
        toNote.IsPinned = fromNote.IsPinned;
        toNote.IsArchived = fromNote.IsArchived;
        toNote.Color = fromNote.Color ?? default!;
    }
}