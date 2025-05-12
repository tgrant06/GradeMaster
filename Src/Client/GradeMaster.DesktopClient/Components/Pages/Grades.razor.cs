using BlazorBootstrap;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;

namespace GradeMaster.DesktopClient.Components.Pages;

public partial class Grades : IAsyncDisposable
{
    #region Fields / Properties

    private string _searchValue = string.Empty;

    private bool _existsAnySubjectInProgress;

    private int _totalItemCount;

    private Virtualize<Grade>? _virtualizeComponent;

    private ConfirmDialog _dialog = default!;

    private DotNetObjectReference<Grades>? _objRef;

    #endregion

    #region Dependency Injection

    [Inject]
    private IGradeRepository _gradeRepository
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
        await JSRuntime.InvokeVoidAsync("addPageKeybinds", "GradesPage", _objRef);

        await _weightRepository.GetAllAsync();
        _existsAnySubjectInProgress = await _subjectRepository.ExistsAnyIsCompletedAsync(false);

        var uri = new Uri(Navigation.Uri);
        var queryParams = QueryHelpers.ParseQuery(uri.Query);
        if (queryParams.TryGetValue("q", out var searchValue))
        {
            var searchValueString = searchValue.ToString();
            _searchValue = string.IsNullOrWhiteSpace(searchValueString) ? string.Empty : searchValueString;
        }

        _totalItemCount = await _gradeRepository.GetTotalCountAsync(_searchValue);
    }

    #region Data

    private async ValueTask<ItemsProviderResult<Grade>> GetGradesProvider(ItemsProviderRequest request)
    {
        var startIndex = request.StartIndex;
        var count = request.Count;

        // Fetch only the required slice of data
        var fetchedGrades = await _gradeRepository.GetBySearchWithRangeAsync(_searchValue, startIndex, count);

        // Calculate the total number of items (if known or needed)
        //var totalItemCount = await _gradeRepository.GetTotalCountAsync(_searchValue);

        // Return the result to the Virtualize component
        return new ItemsProviderResult<Grade>(fetchedGrades, _totalItemCount);
    }

    private async Task RefreshGradeData()
    {
        var uri = new Uri(Navigation.Uri);
        var baseUri = uri.GetLeftPart(UriPartial.Path);
        var updatedUri = QueryHelpers.AddQueryString(baseUri, "q", _searchValue);
        Navigation.NavigateTo(updatedUri, forceLoad: false);

        _totalItemCount = await _gradeRepository.GetTotalCountAsync(_searchValue);

        await _virtualizeComponent?.RefreshDataAsync()!;
    }

    private async Task LoadAllGrades()
    {
        _searchValue = string.Empty;
        await RefreshGradeData();
    }

    #endregion

    #region Not Used

    // protected override async Task OnAfterRenderAsync(bool firstRender)
    // {
    //     if (firstRender)
    //     {
    //         await JSRuntime.InvokeVoidAsync("initializeGridAnimation");
    //     }
    // }

    // private async Task LoadGrades()
    // {
    //     _grades = await _gradeRepository.GetAllAsync();
    //     _filteredGrades = _grades;
    // }

    // private async Task FilterGrades()
    // {
    //     await _virtualizeComponent?.RefreshDataAsync();

    //     if (string.IsNullOrWhiteSpace(_searchValue))
    //     {
    //         _filteredGrades = _grades;
    //     }
    //     else
    //     {
    //         _filteredGrades = _grades
    //             .Where(grade =>
    //                 grade.Subject.Name.Contains(_searchValue, StringComparison.OrdinalIgnoreCase) ||
    //                 (grade.Description != null && grade.Description.Contains(_searchValue, StringComparison.OrdinalIgnoreCase)) ||
    //                 grade.Value.ToString().Contains(_searchValue, StringComparison.OrdinalIgnoreCase) ||
    //                 grade.Subject.Education.Name.Contains(_searchValue, StringComparison.OrdinalIgnoreCase) ||
    //                 grade.Date.ToString("yy-MM-dd").Contains(_searchValue, StringComparison.OrdinalIgnoreCase))
    //             .ToList();
    //     }
    // }

    // private async Task HandleGradeDeleted(int gradeId)
    // {
    //     var grade = _filteredGrades.FirstOrDefault(g => g.Id == gradeId);
    //     if (grade != null)
    //     {
    //         _filteredGrades.Remove(grade);
    //     }

    //     await _virtualizeComponent?.RefreshDataAsync();
    // }

    #endregion

    #region Navigation

    private void CreateGrade() => Navigation.NavigateTo("/grades/create");

    #endregion

    #region JSInvokable / Keybinds

    [JSInvokable]
    public void NavigateToCreate() {
        if (_existsAnySubjectInProgress)
        {
            CreateGrade();
            return;
        }

        ToastService.Notify(new ToastMessage(ToastType.Info, $"Create Education first"));
    }

    [JSInvokable]
    public async Task ClearSearch()
    {
        await LoadAllGrades();
        StateHasChanged();
    }

    #endregion

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("removePageKeybinds", "GradesPage");
        _objRef?.Dispose();
    }
}