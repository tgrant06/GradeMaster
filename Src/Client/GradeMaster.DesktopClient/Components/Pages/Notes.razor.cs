using BlazorBootstrap;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;

namespace GradeMaster.DesktopClient.Components.Pages;

public partial class Notes : IAsyncDisposable
{
    #region Fields / Properties

    private string _searchValue = string.Empty;

    private int _totalItemCount;

    private int _allArchivedNotesCount;

    private Virtualize<Note>? _virtualizeComponent;

    private ConfirmDialog _dialog = default!;

    private DotNetObjectReference<Notes>? _objRef;

    #endregion

    #region Dependency Injection

    [Inject]
    private INoteRepository _noteRepository
    {
        get; set;
    }

    [Inject]
    private IColorRepository _colorRepository
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
        _objRef = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("addPageKeybinds", "NotesPage", _objRef);

        await _colorRepository.GetAllAsync();

        var uri = new Uri(Navigation.Uri);
        var queryParams = QueryHelpers.ParseQuery(uri.Query);
        if (queryParams.TryGetValue("q", out var searchValue))
        {
            var searchValueString = searchValue.ToString();
            _searchValue = string.IsNullOrWhiteSpace(searchValueString) ? string.Empty : searchValueString;
        }

        _totalItemCount = await _noteRepository.GetTotalCountAsync(_searchValue);

        _allArchivedNotesCount = await _noteRepository.GetTotalArchivedNotesCountAsync();
    }

    #region Data

    private async ValueTask<ItemsProviderResult<Note>> GetNotesProvider(ItemsProviderRequest request)
    {
        var startIndex = request.StartIndex;
        var count = request.Count;

        // Fetch only the required slice of data
        var fetchedNotes = await _noteRepository.GetBySearchWithRangeAsync(_searchValue, startIndex, count);

        // Return the result to the Virtualize component
        return new ItemsProviderResult<Note>(fetchedNotes, _totalItemCount);
    }

    private async Task RefreshNoteDataFromCard()
    {
        await RefreshNoteData();
        _allArchivedNotesCount = await _noteRepository.GetTotalArchivedNotesCountAsync();
    }

    private async Task RefreshNoteData()
    {
        var uri = new Uri(Navigation.Uri);
        var baseUri = uri.GetLeftPart(UriPartial.Path);
        var updatedUri = QueryHelpers.AddQueryString(baseUri, "q", _searchValue);
        Navigation.NavigateTo(updatedUri, forceLoad: false);

        _totalItemCount = await _noteRepository.GetTotalCountAsync(_searchValue);

        await _virtualizeComponent?.RefreshDataAsync()!;
    }

    private async Task LoadAllNotes()
    {
        _searchValue = string.Empty;
        await RefreshNoteData();
    }

    #endregion

    #region Navigation

    private void CreateNote() => Navigation.NavigateTo("/notes/create");

    #endregion

    #region JSInvokable / Keybinds

    [JSInvokable]
    public void NavigateToCreate()
    {
        CreateNote();
        //ToastService.Notify(new ToastMessage(ToastType.Info, $"Create Education first"));
    }

    [JSInvokable]
    public async Task ClearSearch()
    {
        await LoadAllNotes();
        StateHasChanged();
    }

    #endregion

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("removePageKeybinds", "NotesPage");
        _objRef?.Dispose();
    }
}