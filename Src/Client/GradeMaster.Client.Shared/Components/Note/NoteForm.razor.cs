using BlazorBootstrap;
using GradeMaster.Client.Shared.Components.Grade;
using GradeMaster.Common.Entities;
using GradeMaster.Common.Enums;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using GradeMaster.Logic.Interfaces.IServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

using Entities = GradeMaster.Common.Entities;

namespace GradeMaster.Client.Shared.Components.Note;

public partial class NoteForm
{
    #region Fields / Properties

    [Parameter]
    public FormType FormType
    {
        get; set;
    }

    [Parameter]
    public Entities.Note? Note
    {
        get; set;
    }

    [Parameter]
    public EventCallback<Entities.Note> OnSave
    {
        get; set;
    }

    private string FormTitle { get; set; } = "Note Form";

    public Entities.Note NewNote { get; set; } = new();

    public List<Color> Colors { get; set; } = new();

    public List<Entities.Subject> Subjects { get; set; } = new();

    private int SelectedColorId
    {
        get => NewNote.Color?.Id ?? 0;
        set
        {
            if (Colors.Count != 0)
            {
                NewNote.Color = Colors.FirstOrDefault(e => e.Id == value)!;
            }
        }
    }

    private EditContext? _editContext;

    private DotNetObjectReference<NoteForm>? _objRef;

    #endregion

    #region Dependency Injection

    [Inject]
    private IJSRuntime JSRuntime
    {
        get; set;
    }

    [Inject]
    private NavigationManager Navigation
    {
        get; set;
    }

    [Inject]
    private INoteService _noteService
    {
        get; set;
    }


    [Inject]
    private IColorRepository _colorRepository
    {
        get; set;
    }

    [Inject] protected ToastService ToastService { get; set; } = default!;

    #endregion

    protected async override Task OnInitializedAsync()
    {
        try
        {
            Colors = await _colorRepository.GetAllAsync();

            if (Colors == null || Colors.Count == 0)
            {
                throw new InvalidOperationException("No weights found.");
            }

            //if (!SubjectId.HasValue)
            //{
            //    if (EducationId.HasValue)
            //    {
            //        if (SubjectSemester is > 0)
            //        {
            //            Subjects = await _subjectRepository.GetByEducationIdAndCompletedWithSemesterAsync(EducationId.Value, false, SubjectSemester.Value);
            //        }
            //        else
            //        {
            //            Subjects = await _subjectRepository.GetByEducationIdAndCompletedAsync(EducationId.Value, false);
            //        }

            //        Subjects = Subjects.OrderByDescending(s => s.Semester).ThenBy(s => s.Name).ThenByDescending(s => s.Id).ToList();
            //    }
            //    else
            //    {
            //        Subjects = await _subjectRepository.GetByCompletedAsync(false);

            //        Subjects = Subjects.OrderByDescending(s => s.Id).ToList();
            //    }

            //    //Subjects = Subjects.OrderByDescending(s => s.Id).ToList();
            //    if (FormType == FormType.Edit && Note != null)
            //    {
            //        // can never be null
            //        var subject = await _subjectRepository.GetByGradeIdAsync(Note.Id);

            //        if (!Subjects.Exists(e => e.Id == subject.Id))
            //        {
            //            Subjects.Insert(0, subject);
            //        }
            //    }
            //}
            //else
            //{
            //    Subjects.Add(await _subjectRepository.GetByIdDetailAsync(SubjectId.Value));
            //}

            //if (Subjects == null || Subjects.Count == 0)
            //{
            //    throw new InvalidOperationException("No subjects found.");
            //}

            if (Note == null)
            {
                // Initialize Grade for creation
                Note = new Entities.Note
                {
                    Color = Colors.Find(c => c.Id == 1)!
                };

                FormTitle = "Create New Note";
            }
            else
            {
                FormTitle = "Edit Note";
            }

            _noteService.PassObjectAttributes(NewNote, Note);

            _editContext = new EditContext(NewNote);
        }
        catch (Exception ex)
        {
            ToastService.Notify(new ToastMessage(ToastType.Danger, $"Error during initialization: {ex.Message}"));
            throw;
        }

        _objRef = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("addPageKeybinds", "FormComponent", _objRef);
    }

    #region HandleSubmit

    private async Task HandleValidSubmit()
    {
        if (FormType == FormType.Create)
        {
            NewNote.CreatedAt = DateTime.Now;
        }

        NewNote.UpdatedAt = DateTime.Now;

        if (NewNote.Value < 1)
        {
            NewNote.Value = 1;
            ToastService.Notify(new ToastMessage(ToastType.Warning, "Please enter a valid grade value (1 - 6)."));
            return;
        }

        if (NewNote.Weight == null || NewNote.Weight.Id == 0)
        {
            ToastService.Notify(new ToastMessage(ToastType.Warning, "Please select a valid weight."));
            return;
        }

        if (NewNote.Subject == null || NewNote.Subject.Id == 0)
        {
            ToastService.Notify(new ToastMessage(ToastType.Warning, "Please select a valid subject."));
            return;
        }

        if (NewNote.Date > NewNote.Subject.Education.EndDate)
        {
            NewNote.Date = NewNote.Subject.Education.EndDate;
            ToastService.Notify(new ToastMessage(ToastType.Warning, "Date of grade may not exceed education end date."));
            return;
        }

        if (NewNote.Date < NewNote.Subject.Education.StartDate)
        {
            NewNote.Date = NewNote.Subject.Education.StartDate;
            ToastService.Notify(new ToastMessage(ToastType.Warning, "Date of grade may not be below education start date."));
            return;
        }

        if (OnSave.HasDelegate)
        {
            _noteService.PassObjectAttributes(Note, NewNote, true);
            await OnSave.InvokeAsync(Note);
        }
        else
        {
            // Optional fallback logic
            ToastService.Notify(new ToastMessage(ToastType.Danger, $"Grade could not be saved."));
        }
    }

    private void HandleInvalidSubmit()
    {
        ToastService.Notify(new ToastMessage(ToastType.Warning, $"Grade form is not valid."));
    }

    #endregion

    private async Task Cancel() => await JSRuntime.InvokeVoidAsync("window.history.back");

    #region JSInvokable / Keybinds

    [JSInvokable]
    public async Task SubmitForm()
    {
        var isValid = _editContext?.Validate();

        if (isValid == true)
        {
            await HandleValidSubmit();
        }
        else
        {
            HandleInvalidSubmit();
        }
    }

    #endregion

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("removePageKeybinds", "FormComponent");
        _objRef?.Dispose();
    }
}