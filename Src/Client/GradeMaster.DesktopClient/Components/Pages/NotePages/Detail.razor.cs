using System.Diagnostics.Metrics;
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

    private int _descriptionAreaExpandedHeight;

    private ConfirmDialog _dialog = default!;

    private BlazorBootstrap.Button _copyButton = default!;

    private DotNetObjectReference<Detail>? _objRef;

    #endregion

    #region Dependency Injection
    [Inject]
    private IEducationRepository _educationRepository
    {
        get; set;
    }

    [Inject]
    private ISubjectRepository _subjectRepository
    {
        get; set;
    }

    [Inject]
    private IGradeRepository _gradeRepository
    {
        get; set;
    }

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
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("attachLinkInterceptor", _objRef);
        }
    }

    #region Content

    private async Task ShowNoteContent()
    {
        await JSRuntime.InvokeVoidAsync("window.scrollToContentSection");
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
            message1: $"Note: {Note.Title} with color: {Note.Color.Name} {Note.Color.Symbol}, created at: {Note.CreatedAt:f} and last updated at: {Note.UpdatedAt:f} will be deleted.",
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

    #region Clipboard

    private async Task CopyToClipboard()
    {
        _copyButton.ShowLoading();

        var textToCopy = $"/notes/{Note.Id}";
        //await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", textToCopy);
        await Clipboard.SetTextAsync(textToCopy);

        ToastService.Notify(new ToastMessage(ToastType.Success, "Copied page URL to clipboard"));

        await Task.Delay(1500);

        _copyButton.HideLoading();
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
    public async Task NavigateFromJs(string? url)
    {
        if (url is null)
        {
            ToastInvalidUrl("URL is null!");
            return;
        }

        // handle when linking home page ("/?...") or other non-entity URLs

        var urlArray = url.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (urlArray.Length == 0)
        {
            ToastInvalidUrl("'" + url + "'");
            return;
        }

        var stringId = urlArray.Last();
        if (!int.TryParse(stringId, out var id))
        {
            ToastInvalidUrl("'" + url + "'");
            return;
        }

        if (url.StartsWith("/notes", StringComparison.OrdinalIgnoreCase))
        {
            if (!await _noteRepository.ExistsAsync(id))
            {
                ToastService.Notify(new ToastMessage(ToastType.Info, $"Note with Id: '{id}' doesn't exist."));
                return;
            }
            Navigation.NavigateTo(url, true);
            return;
        } 

        if (url.StartsWith("/educations", StringComparison.OrdinalIgnoreCase))
        {
            if (!await _educationRepository.ExistsAsync(id))
            {
                ToastService.Notify(new ToastMessage(ToastType.Info, $"Education with Id: '{id}' doesn't exist."));
                return;
            }
            Navigation.NavigateTo(url);
            return;
        }

        if (url.StartsWith("/subjects", StringComparison.OrdinalIgnoreCase))
        {
            if (!await _subjectRepository.ExistsAsync(id))
            {
                ToastService.Notify(new ToastMessage(ToastType.Info, $"Subject with Id: '{id}' doesn't exist."));
                return;
            }
            Navigation.NavigateTo(url);
            return;
        }

        if (url.StartsWith("/grades", StringComparison.OrdinalIgnoreCase))
        {
            if (!await _gradeRepository.ExistsAsync(id))
            {
                ToastService.Notify(new ToastMessage(ToastType.Info, $"Grade with Id: '{id}' doesn't exist."));
                return;
            }
            Navigation.NavigateTo(url);
            return;
        }

        ToastInvalidUrl(url);
    }

    [JSInvokable]
    public async Task CopyPageUrl() => await CopyToClipboard();

    #endregion

    #region Utility

    private void ToastInvalidUrl(string url)
    {
        ToastService.Notify(new ToastMessage(ToastType.Warning, $"Invalid URL. URL: {url}."));
    }

    #endregion

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("removePageKeybinds", "NoteDetailPage");
        await JSRuntime.InvokeVoidAsync("detachLinkInterceptor");
        _objRef?.Dispose();
    }
}