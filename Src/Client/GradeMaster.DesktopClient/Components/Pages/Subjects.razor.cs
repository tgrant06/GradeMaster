using BlazorBootstrap;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;

namespace GradeMaster.DesktopClient.Components.Pages;

public partial class Subjects : IAsyncDisposable
{
    #region Fields / Properties

    private string _searchValue = string.Empty;

    private bool _existsAnyEducationInProgress;

    private Virtualize<Subject>? _virtualizeComponent;

    private ConfirmDialog _dialog = default!;

    private DotNetObjectReference<Subjects>? _objRef;

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
    private IWeightRepository _weightRepository
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
        await JSRuntime.InvokeVoidAsync("addPageKeybinds", "SubjectsPage", _objRef);

        await _weightRepository.GetAllAsync();
        _existsAnyEducationInProgress = await _educationRepository.ExistsAnyIsCompletedAsync(false);

        var uri = new Uri(Navigation.Uri);
        var queryParams = QueryHelpers.ParseQuery(uri.Query);
        if (queryParams.TryGetValue("q", out var searchValue))
        {
            var searchValueString = searchValue.ToString();
            _searchValue = string.IsNullOrWhiteSpace(searchValueString) ? string.Empty : searchValueString;
        }
    }

    #region Data

    private async ValueTask<ItemsProviderResult<Subject>> GetSubjectsProvider(ItemsProviderRequest request)
    {
        var startIndex = request.StartIndex;
        var count = request.Count;

        // Fetch only the required slice of data
        var fetchedSubjects = await _subjectRepository.GetBySearchWithRangeAsync(_searchValue, startIndex, count);

        // Calculate the total number of items (if known or needed)
        var totalItemCount = await _subjectRepository.GetTotalCountAsync(_searchValue);

        // Return the result to the Virtualize component
        return new ItemsProviderResult<Subject>(fetchedSubjects, totalItemCount);
    }

    private async Task RefreshSubjectData()
    {
        var uri = new Uri(Navigation.Uri);
        var baseUri = uri.GetLeftPart(UriPartial.Path);
        var updatedUri = QueryHelpers.AddQueryString(baseUri, "q", _searchValue);
        Navigation.NavigateTo(updatedUri, forceLoad: false);

        await _virtualizeComponent?.RefreshDataAsync()!;
    }

    private async Task LoadAllSubjects()
    {
        _searchValue = string.Empty;
        await RefreshSubjectData();
    }

    #endregion

    #region Not Used

    // private async Task LoadSubjects()
    // {
    //     _subjects = await _subjectRepository.GetAllAsync();
    //     //await _educationRepository.GetAllAsync();
    //     _filteredSubjects = _subjects;
    // }

    // private async Task FilterSubjects()
    // {
    //     await _virtualizeComponent?.RefreshDataAsync();

    //     if (string.IsNullOrWhiteSpace(_searchValue))
    //     {
    //         _filteredSubjects = _subjects;
    //     }
    //     else
    //     {
    //         _filteredSubjects = _subjects
    //             .Where(subject =>
    //                 subject.Name.Contains(_searchValue, StringComparison.OrdinalIgnoreCase) ||
    //                 (subject.Description != null && subject.Description.Contains(_searchValue, StringComparison.OrdinalIgnoreCase)) ||
    //                 subject.Semester.ToString().Contains(_searchValue, StringComparison.OrdinalIgnoreCase) ||
    //                 subject.Education.Name.Contains(_searchValue, StringComparison.OrdinalIgnoreCase))
    //             .ToList();
    //     }
    // }

    // private async Task HandleSubjectDeleted(int subjectId)
    // {
    //     var subject = _filteredSubjects.FirstOrDefault(s => s.Id == subjectId);
    //     if (subject != null)
    //     {
    //         _filteredSubjects.Remove(subject);
    //     }

    //     await _virtualizeComponent?.RefreshDataAsync();
    // }

    #endregion

    #region Navigation

    private void CreateSubject() => Navigation.NavigateTo("/subjects/create");

    #endregion

    #region JSInvokable / Keybinds

    [JSInvokable]
    public void NavigateToCreate()
    {
        if (_existsAnyEducationInProgress)
        {
            CreateSubject();
            return;
        }

        ToastService.Notify(new ToastMessage(ToastType.Info, $"Create Education first"));
    }

    [JSInvokable]
    public async Task ClearSearch()
    {
        await LoadAllSubjects();
        StateHasChanged();
    }

    #endregion

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("removePageKeybinds", "SubjectsPage");
        _objRef?.Dispose();
    }
}