using GradeMaster.Client.Shared.Utility;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace GradeMaster.DesktopClient.Components.Pages;

public partial class Home
{
    private int _currentEducationId;
    private List<Education> _educations = new();
    private List<Subject> _subjects = new();

    private decimal _educationAverage = new();
    private int? _selectedEducationId;

    #region Dependancy Injection

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
        }
    }

    private async Task LoadEducationData(int educationId)
    {
        _currentEducationId = educationId;
        _subjects = await _subjectRepository.GetByEducationIdAsync(educationId); // get weight separately
        _educationAverage = EducationUtils.CalculateEducationAverage(_subjects);
    }

    private void GoToSubject(int subjectId)
    {
        Navigation.NavigateTo($"/subjects/{subjectId}");
    }

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

    private async Task ReloadData()
    {
        if (_selectedEducationId.HasValue)
        {
            await LoadEducationData(_selectedEducationId.Value);
        }
    }
}