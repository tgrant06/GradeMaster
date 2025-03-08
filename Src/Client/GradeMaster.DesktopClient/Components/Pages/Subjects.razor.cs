using BlazorBootstrap;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components;

namespace GradeMaster.DesktopClient.Components.Pages;

public partial class Subjects
{
    #region Fields / Properties

    private string _searchValue = string.Empty;
    private bool _existsAnyEducationInProgress;

    private Virtualize<Subject>? _virtualizeComponent;

    private ConfirmDialog _dialog = default!;

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

    [Inject]
    private NavigationManager Navigation
    {
        get; set;
    }

    #endregion

    protected async override Task OnInitializedAsync()
    {
        await _weightRepository.GetAllAsync();
        _existsAnyEducationInProgress = await _educationRepository.ExistsAnyIsCompletedAsync(false);
    }

    private async ValueTask<ItemsProviderResult<Subject>> GetSubjectsProvider(ItemsProviderRequest request)
    {
        var startIndex = request.StartIndex;
        var count = request.Count;

        // Fetch only the required slice of data
        var fetchedSubjects = await _subjectRepository.GetBySearchWithLimitAsync(_searchValue, startIndex, count);

        // Calculate the total number of items (if known or needed)
        var totalItemCount = await _subjectRepository.GetTotalCountAsync(_searchValue);

        // Return the result to the Virtualize component
        return new ItemsProviderResult<Subject>(fetchedSubjects, totalItemCount);
    }

    private async Task RefreshSubjectData()
    {
        await _virtualizeComponent?.RefreshDataAsync();
    }

    private async Task LoadAllSubjects()
    {
        _searchValue = string.Empty;
        await RefreshSubjectData();
    }

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
}