using BlazorBootstrap;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components;

namespace GradeMaster.DesktopClient.Components.Pages;

public partial class Grades
{
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

    [Inject]
    private NavigationManager Navigation
    {
        get; set;
    }

    #endregion

    private string _searchValue = string.Empty;
    private bool _existsAnySubjectInProgress;

    private Virtualize<Grade>? _virtualizeComponent;

    private ConfirmDialog _dialog = default!;

    private async ValueTask<ItemsProviderResult<Grade>> GetGradesProvider(ItemsProviderRequest request)
    {
        var startIndex = request.StartIndex;
        var count = request.Count;

        // Fetch only the required slice of data
        var fetchedGrades = await _gradeRepository.GetBySearchWithLimitAsync(_searchValue, startIndex, count);

        // Calculate the total number of items (if known or needed)
        var totalItemCount = await _gradeRepository.GetTotalCountAsync(_searchValue);

        // Return the result to the Virtualize component
        return new ItemsProviderResult<Grade>(fetchedGrades, totalItemCount);
    }

    protected async override Task OnInitializedAsync()
    {
        await _weightRepository.GetAllAsync();
        _existsAnySubjectInProgress = await _subjectRepository.ExistsAnyIsCompletedAsync(false);
        // await LoadGrades();
    }

    private async Task RefreshGradeData()
    {
        await _virtualizeComponent?.RefreshDataAsync();
    }

    private async Task LoadAllGrades()
    {
        _searchValue = string.Empty;
        await _virtualizeComponent?.RefreshDataAsync();
        //await LoadGrades();
    }

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

    private void CreateGrade()
    {
        Navigation.NavigateTo("/grades/create");
    }
}