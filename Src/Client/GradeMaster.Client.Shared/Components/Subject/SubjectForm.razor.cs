using BlazorBootstrap;
using GradeMaster.Common.Enums;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using GradeMaster.Logic.Interfaces.IServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using Entities = GradeMaster.Common.Entities;
using GradeMaster.Client.Shared.Components.Education;

namespace GradeMaster.Client.Shared.Components.Subject;

public partial class SubjectForm : IAsyncDisposable
{
    #region Fields / Properties

    [Parameter]
    public FormType FormType
    {
        get; set;
    }

    [Parameter]
    public Entities.Subject? Subject
    {
        get; set;
    }

    [Parameter]
    public EventCallback<Entities.Subject> OnSave
    {
        get; set;
    }

    [Parameter]
    public int? EducationId
    {
        get; set;
    }

    private int SelectedEducationId
    {
        get => NewSubject.Education?.Id ?? 0;
        set
        {
            if (Educations.Count != 0) 
            {
                NewSubject.Education = Educations.FirstOrDefault(e => e.Id == value)!;
            }
        }
    }

    private string FormTitle { get; set; } = "Subject Form";

    public Entities.Subject NewSubject { get; set; } = new();

    public List<Entities.Education> Educations { get; set; } = new();

    private EditContext? _editContext;

    private DotNetObjectReference<SubjectForm>? _objRef;

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
    private ISubjectService _subjectService
    {
        get; set;
    }

    [Inject]
    private IEducationRepository _educationRepository
    {
        get; set;
    }

    [Inject] protected ToastService ToastService { get; set; } = default!;

    #endregion

    protected async override Task OnInitializedAsync()
    {
        try
        {
            if (!EducationId.HasValue)
            {
                Educations = await _educationRepository.GetByCompletedAsync(false);
                Educations = Educations.OrderByDescending(e => e.Id).ToList();
                if (FormType == FormType.Edit && Subject != null)
                {
                    // can never be null
                    var education = await _educationRepository.GetBySubjectIdAsync(Subject.Id);

                    if (!Educations.Exists(e => e.Id == education.Id))
                    {
                        Educations.Insert(0, education);
                    }
                }
            }
            else
            {
                Educations.Add(await _educationRepository.GetByIdAsync(EducationId.Value));
            }

            if (Educations == null || Educations.Count == 0)
            {
                throw new InvalidOperationException("No educations found.");
            }

            if (Subject == null)
            {
                // Initialize Education for creation
                Subject = new Entities.Subject
                {
                    Completed = false,
                    Semester = 1,
                    Education = EducationId.HasValue ? Educations.First() : default!
                };

                FormTitle = "Create New Subject";
            }
            else
            {
                FormTitle = "Edit Subject";
            }

            _subjectService.PassObjectAttributes(NewSubject, Subject);

            _editContext = new EditContext(NewSubject);
        }
        catch (Exception ex)
        {
            ToastService.Notify(new ToastMessage(ToastType.Danger, $"Error during initialization: {ex.Message}"));
            throw;
        }

        _objRef = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("addPageKeybinds", "FormComponent", _objRef);
    }

    private int GetMaxSemesterNumber()
    {
        var education = Educations.FirstOrDefault(e => e.Id == SelectedEducationId);

        if (education == null)
        {
            return 256;
        }

        return education.Semesters;
    }

    #region HandleSubmit

    private async Task HandleValidSubmit()
    {
        if (NewSubject.Education == null || NewSubject.Education.Id == 0)
        {
            ToastService.Notify(new ToastMessage(ToastType.Warning, "Please select a valid education."));
            return;
        }

        if (NewSubject.Semester > NewSubject.Education.Semesters)
        {
            NewSubject.Semester = NewSubject.Education.Semesters;
            ToastService.Notify(new ToastMessage(ToastType.Warning, $"Please select a valid semester between 1 and {NewSubject.Education.Semesters}."));
            return;
        }

        if (OnSave.HasDelegate)
        {
            _subjectService.PassObjectAttributes(Subject, NewSubject, true);
            await OnSave.InvokeAsync(Subject);
        }
        else
        {
            // Optional fallback logic
            ToastService.Notify(new ToastMessage(ToastType.Danger, $"Subject could not be saved."));
        }
    }

    private void HandleInvalidSubmit()
    {
        ToastService.Notify(new ToastMessage(ToastType.Warning, $"Subject form is not valid."));
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