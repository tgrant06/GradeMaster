using BlazorBootstrap;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Entities = GradeMaster.Common.Entities;

namespace GradeMaster.DesktopClient.Components.Pages.NotePages;

public partial class Detail : IAsyncDisposable
{
    #region Fields / Properties

    [Parameter]
    public int Id
    {
        get; set;
    }

    public Note Note { get; set; } = new();

    private bool IsExpanded { get; set; } = false;

    private bool IsTruncated { get; set; } = false;

    private string ButtonText => IsExpanded ? "less" : "...more";

    private string DescriptionAreaDynamicHeight => IsExpanded ? $"max-height: {_descriptionAreaExpandedHeight}px;" : "max-height: 175px;";

    private int _descriptionAreaExpandedHeight;

    private ConfirmDialog _dialog = default!;

    private DotNetObjectReference<Detail>? _objRef;

    #endregion

    #region Dependency Injection

    [Inject]
    private INoteRepository _noteRepository
    {
        get; set;
    }

    [Inject] protected ToastService ToastService { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation
    {
        get; set;
    }

    [Inject]
    private IJSRuntime JSRuntime
    {
        get; set;
    }

    #endregion

    protected async override Task OnInitializedAsync()
    {
        // Initialize to avoid null reference
        Note.Color = new Entities.Color();

        var noteExists = await _noteRepository.ExistsAsync(Id);

        if (!noteExists)
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, $"This note does no longer exist."));
            await GoBack();
            return;
        }

        _objRef = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("addPageKeybinds", "NoteDetailPage", _objRef);

        Note = await _noteRepository.GetByIdDetailAsync(Id);

        // Calculate the average only after loading the data
        //await CalculateSubjectAverage();
    }

    #region Description

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            IsTruncated = await JSRuntime.InvokeAsync<bool>("checkDescriptionHeight", "description-area", 175);
            await SetDescriptionAreaExpandedHeight();
            StateHasChanged();
        }
    }

    private async Task ToggleDescription()
    {
        if (!IsExpanded)
        {
            await SetDescriptionAreaExpandedHeight();
        }

        IsExpanded = !IsExpanded;

        await DynamicDescriptionHeightActive(IsExpanded);
    }

    private async Task SetDescriptionAreaExpandedHeight()
    {
        _descriptionAreaExpandedHeight = await JSRuntime.InvokeAsync<int>("getMaxDescriptionHeight", "description-text");
    }

    private async Task DynamicDescriptionHeightActive(bool isActive)
    {
        if (isActive)
        {
            await JSRuntime.InvokeVoidAsync("addDescriptionAreaEventListener");
        }
        else
        {
            await JSRuntime.InvokeVoidAsync("removeDescriptionAreaEventListener");
        }
    }

    #endregion

    #region Delete Note

    private async Task DeleteNoteAsync()
    {
        var options = new ConfirmDialogOptions
        {
            YesButtonColor = ButtonColor.Danger,
        };

        var confirmation = await _dialog.ShowAsync(
            title: "Are you sure you want to delete this Note?",
            message1: $"Note: {Note.Title} with color {Note.Color.Name} {Note.Color.Symbol}, created at: {Note.CreatedAt:f} and last updated at: {Note.UpdatedAt:f} will be deleted.",
            message2: "Do you want to proceed?",
            confirmDialogOptions: options);

        if (confirmation)
        {
            try
            {
                await _noteRepository.DeleteByIdAsync(Note.Id);
                await GoBack();
                ToastService.Notify(new ToastMessage(ToastType.Success, $"Note deleted successfully.")); // maybe add Name of deleted object
            }
            catch (Exception e)
            {
                ToastService.Notify(new ToastMessage(ToastType.Danger, $"Error deleting note: {e.Message}"));
            }
        }
    }

    #endregion

    #region Navigation

    private async Task GoBack() => await JSRuntime.InvokeVoidAsync("window.history.back");

    private void EditNote() => Navigation.NavigateTo($"/notes/{Note.Id}/edit");

    #endregion

    #region JSInvokable / Keybinds

    [JSInvokable]
    public void NavigateToEdit() => EditNote();

    [JSInvokable]
    public async Task DeleteObject() => await DeleteNoteAsync();

    [JSInvokable]
    public async Task ToggleDescriptionHeight()
    {
        if (IsTruncated)
        {
            await ToggleDescription();
            StateHasChanged();
        }
    }

    #endregion

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("removeDescriptionAreaEventListener");

        await JSRuntime.InvokeVoidAsync("removePageKeybinds", "NoteDetailPage");
        _objRef?.Dispose();
    }
}