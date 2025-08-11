using BlazorBootstrap;
using GradeMaster.Common.Entities;
using GradeMaster.Common.Enums;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using GradeMaster.Logic.Interfaces.IServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

using Entities = GradeMaster.Common.Entities;

namespace GradeMaster.Client.Shared.Components.Note;

public partial class NoteForm : IAsyncDisposable
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
                throw new InvalidOperationException("No colors found.");
            }

            if (Note == null)
            {
                // Initialize Note for creation
                Note = new Entities.Note
                {
                    Color = Colors.Find(c => c.Id == 1) ?? default!,
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
        await JSRuntime.InvokeVoidAsync("addPageKeybinds", "NoteFormComponent", _objRef);
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("attachLinkInterceptor", _objRef);
        }
    }

    #region HandleSubmit

    private async Task HandleValidSubmit()
    {
        if (FormType == FormType.Create)
        {
            NewNote.CreatedAt = DateTime.Now;
        }

        if (!string.IsNullOrWhiteSpace(NewNote.Tags))
        {
            var tags = NewNote.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            NewNote.Tags = string.Join(", ", tags);
        }

        NewNote.UpdatedAt = DateTime.Now;

        if (OnSave.HasDelegate)
        {
            _noteService.PassObjectAttributes(Note, NewNote, true);
            await OnSave.InvokeAsync(Note);
        }
        else
        {
            // Optional fallback logic
            ToastService.Notify(new ToastMessage(ToastType.Danger, $"Note could not be saved."));
        }
    }

    private void HandleInvalidSubmit()
    {
        ToastService.Notify(new ToastMessage(ToastType.Warning, $"Note form is not valid."));
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
        await JSRuntime.InvokeVoidAsync("removePageKeybinds", "NoteFormComponent");
        await JSRuntime.InvokeVoidAsync("detachLinkInterceptor");
        _objRef?.Dispose();
    }
}