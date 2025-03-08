using GradeMaster.Client.Shared.Utility;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace GradeMaster.DesktopClient.Components.Pages;

public partial class Home
{
    #region Fields / Properties

    private int _currentEducationId;
    private List<Education> _educations = new();
    private List<Subject> _subjects = new();

    private decimal _educationAverage = new();
    private int? _selectedEducationId;

    private int? SelectedEducationId
    {
        get => _selectedEducationId;
        set
        {
            if (_selectedEducationId != value)
            {
                _selectedEducationId = value;
                ChangeEducation(value);
            }
        }
    }

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
        _educations = await _educationRepository.GetAllSimpleAsync();
        await _weightRepository.GetAllAsync();

        var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("educationId", out var educationIdString) && int.TryParse(educationIdString, out var educationId))
        {
            _selectedEducationId = educationId;
            await LoadEducationData(educationId);
        }
        else
        {
            // Navigate to the URL with the default query parameter
            Navigation.NavigateTo($"?", false);
        }
    }

    private async void ChangeEducation(int? educationId)
    {
        if (educationId.HasValue)
        {
            await LoadEducationData(educationId.Value);
            Navigation.NavigateTo($"?educationId={educationId}", false);
        }
        else
        {
            _currentEducationId = 0;
            _subjects = new List<Subject>();
            _educationAverage = 0;

            Navigation.NavigateTo($"?", false);
        }
    }

    private async Task LoadEducationData(int educationId)
    {
        _currentEducationId = educationId;
        _subjects = await _subjectRepository.GetByEducationIdAsync(educationId);
        _educationAverage = EducationUtils.CalculateEducationAverage(_subjects);
    }

    // maybe reload entire page? (soft reload)
    // if selectedEducationId has no value then reload page (soft reload) for example
    private async Task ReloadData()
    {
        if (_selectedEducationId.HasValue)
        {
            await LoadEducationData(_selectedEducationId.Value);
        }
        else
        {
            _educations = await _educationRepository.GetAllSimpleAsync();
        }
    }

    #region Navigation

    private void GoToSubject(int subjectId) => Navigation.NavigateTo($"/subjects/{subjectId}");

    private void GoToGrade(int gradeId) => Navigation.NavigateTo($"/grades/{gradeId}");

    private void GoToEducation(int educationId) => Navigation.NavigateTo($"/educations/{educationId}");

    private void GoToNewSubject(int educationId) => Navigation.NavigateTo($"/subjects/create?educationId={educationId}");

    private void GoToNewGrade(int subjectId) => Navigation.NavigateTo($"/grades/create?subjectId={subjectId}");

    #endregion

    #region Averages

    private string GetAverageGrade(ICollection<Grade> grades)
    {
        if (grades != null && grades.Any())
        {
            return SubjectUtils.CalculateWeightedAverage(grades).ToString("0.##");
        }

        return "N/A";
    }

    #endregion
}