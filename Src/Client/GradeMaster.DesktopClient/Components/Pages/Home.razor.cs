using System.Text.RegularExpressions;
using BlazorBootstrap;
using GradeMaster.Client.Shared.Utility;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;

namespace GradeMaster.DesktopClient.Components.Pages;

public partial class Home : IAsyncDisposable
{
    #region Fields / Properties

    private int _currentEducationId;

    private List<Education> _educations = new();

    private List<Subject> _subjects = new();

    private decimal _educationAverage;

    private int? _selectedEducationId;

    private DotNetObjectReference<Home>? _objRef;

    private int _selectedSemester;

    private bool _isFiltered;

    private int _maxSemesterValue;

    private SemesterViewModel _semesterViewModel = new();

    private string _averageDisplayText => _isFiltered ? $"Semester {_selectedSemester} Avg:" : "Education Avg:";

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

    #region Classes 

    private class SemesterViewModel
    {
        public int? Semester
        {
            get; set;
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
        _educations = await _educationRepository.GetAllSimpleAsync();
        await _weightRepository.GetAllAsync();

        _selectedSemester = 0;

        _objRef = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("addPageKeybinds", "HomePage", _objRef);

        var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("educationId", out var educationIdString) && int.TryParse(educationIdString, out var educationId))
        {
            var educationExists = await _educationRepository.ExistsAsync(educationId);

            if (educationExists)
            {
                if (QueryHelpers.ParseQuery(uri.Query)
                        .TryGetValue("semesterFilterNumber", out var semesterFilterNumberString) &&
                    int.TryParse(semesterFilterNumberString, out var semesterFilterNumber))
                {
                    _semesterViewModel.Semester = semesterFilterNumber;
                    _selectedSemester = _semesterViewModel.Semester.Value;
                    _isFiltered = true;
                }

                _selectedEducationId = educationId;
                await LoadEducationData(educationId);
                return;
            }

            ToastService.Notify(new ToastMessage(ToastType.Info, "This education does no longer exist."));
        }

        // Navigate to the URL with the default query parameter
        Navigation.NavigateTo("?", false);
    }

    #region Data

    private async void ChangeEducation(int? educationId)
    {
        _semesterViewModel.Semester = null;
        _isFiltered = false;
        _selectedSemester = 0;

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

            Navigation.NavigateTo("?", false);
        }
    }

    private async Task LoadEducationData(int educationId)
    {
        _currentEducationId = educationId;
        var allSubjects = await _subjectRepository.GetByEducationIdAsync(educationId);
        _subjects = _semesterViewModel.Semester is > 0 ? allSubjects.Where(s => s.Semester == _semesterViewModel.Semester.Value).ToList() : allSubjects;
        _educationAverage = EducationUtils.CalculateEducationAverage(_subjects);
        _maxSemesterValue = GetEducationSemester();
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

    #endregion

    #region Filtering

    private async Task HandleValidSubmit()
    {
        if (!_selectedEducationId.HasValue || _currentEducationId == 0)
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, "Select Education first"));
            return;
        }

        // Get all subjects for the education and filter by semester
        var allSubjects = await _subjectRepository.GetByEducationIdAsync(_currentEducationId);

        if (_semesterViewModel.Semester is > 0)  // Changed from _selectedSemester to SelectedSemester
        {
            _subjects = allSubjects.Where(s => s.Semester == _semesterViewModel.Semester.Value).ToList();  // Changed here too
            _isFiltered = true;
            _selectedSemester = _semesterViewModel.Semester.Value;

            SetSemesterFilterNumberQueryToValue(_semesterViewModel.Semester.Value);

            //if (!_subjects.Any())
            //{
            //    ToastService.Notify(new ToastMessage(ToastType.Info, $"No subjects found for semester {_semesterViewModel.Semester}"));  // Changed here
            //}
        }
        else
        {
            // If semester is 0 or not selected, show all subjects
            _subjects = allSubjects;
            _semesterViewModel.Semester = null;
            _isFiltered = false;
            _selectedSemester = 0;

            SetSemesterFilterNumberQueryToValue();
        }

        // Update the calculated average based on filtered subjects
        _educationAverage = EducationUtils.CalculateEducationAverage(_subjects);
    }

    private async Task ClearFilter()
    {
        _semesterViewModel.Semester = null;
        _isFiltered = false;
        _selectedSemester = 0;

        SetSemesterFilterNumberQueryToValue();

        // Load all subjects for the current education without filtering
        if (_selectedEducationId.HasValue)
        {
            await LoadEducationData(_selectedEducationId.Value);
        }
    }

    #endregion

    #region Clipboard

    private async Task CopyToClipboard()
    {
        var textToCopy = Navigation.ToBaseRelativePath(Navigation.Uri);
        
        await Clipboard.SetTextAsync($"[Home | Education with Id: {_currentEducationId}{(_isFiltered ? ", Selected filter Semester: " + _selectedSemester : "")}](/{textToCopy})");

        ToastService.Notify(new ToastMessage(ToastType.Success, "Copied page link to clipboard"));
    }

    #endregion

    #region Navigation

    private void GoToSubject(int subjectId) => Navigation.NavigateTo($"/subjects/{subjectId}");

    private void GoToGrade(int gradeId) => Navigation.NavigateTo($"/grades/{gradeId}");

    private void GoToEducation(int educationId) => Navigation.NavigateTo($"/educations/{educationId}");

    private void GoToNewSubject(int educationId, int? subjectSemester = null)
    {
        if (subjectSemester is null or 0)
        {
            Navigation.NavigateTo($"/subjects/create?educationId={educationId}");
            return;
        }

        GoToNewSubjectWithSemester(educationId, subjectSemester.Value);
    }

    private void GoToNewSubjectWithSemester(int educationId, int subjectSemester) => Navigation.NavigateTo($"/subjects/create?educationId={educationId}&subjectSemester={subjectSemester}");

    private void GoToNewGrade(int subjectId) => Navigation.NavigateTo($"/grades/create?subjectId={subjectId}");

    private void GoToNewGradeFromEducationId(int educationId) => Navigation.NavigateTo($"/grades/create?educationId={educationId}");

    private void GoToNewGradeFromEducationIdAndSemester(int educationId, int subjectSemester) => Navigation.NavigateTo($"/grades/create?educationId={educationId}&subjectSemester={subjectSemester}");

    private void GoToNewEducation() => Navigation.NavigateTo("/educations/create");

    #endregion

    #region JSInvokable / Keybinds

    [JSInvokable]
    public void NavigateToEducationCreate() => GoToNewEducation();

    [JSInvokable]
    public void NavigateToSubjectCreate()
    {
        if (_currentEducationId == 0)
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, "Select Education first"));
            return;
        }

        if (_educations.Find(e => e.Id.Equals(_currentEducationId))!.Completed)
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, "Education is completed"));
            return;
        }

        if (_semesterViewModel.Semester is > 0)
        {
            GoToNewSubjectWithSemester(_currentEducationId, _semesterViewModel.Semester.Value);
            return;
        }

        GoToNewSubject(_currentEducationId);
    }

    [JSInvokable]
    public async Task NavigateToGradeCreate()
    {
        if (_currentEducationId == 0)
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, "Select Education first"));
            return;
        }

        var educationHasInProgressSubjects =
            await _subjectRepository.ExistsAnyIsCompletedWithEducationIdAsync(_currentEducationId, false);

        if (!educationHasInProgressSubjects)
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, "No 'In Progress' subjects"));
            return;
        }

        if (_semesterViewModel.Semester is > 0)
        {
            if (_subjects.Any(s => s.Completed == false))
            {
                GoToNewGradeFromEducationIdAndSemester(_currentEducationId, _semesterViewModel.Semester.Value);
                return;
            }

            ToastService.Notify(new ToastMessage(ToastType.Info, $"No 'In Progress' subjects with semester {_semesterViewModel.Semester.Value}"));
            return;
        }

        GoToNewGradeFromEducationId(_currentEducationId);
    }

    [JSInvokable]
    public void NavigateToEducationDetail()
    {
        if (_currentEducationId == 0)
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, "Select Education first"));
            return;
        }

        GoToEducation(_currentEducationId);
    }

    [JSInvokable]
    public async Task ReloadPageData()
    {
        await ReloadData();
        StateHasChanged();
    }

    [JSInvokable]
    public async Task ClearFilterState()
    {
        if (!_isFiltered)
        {
            return;
        }

        await ClearFilter();
        StateHasChanged();
    }

    [JSInvokable]
    public async Task CopyPageUrl()
    {
        if (_selectedEducationId.HasValue)
        {
            await CopyToClipboard();
        }
    }

    #endregion

    #region Averages

    private static string GetAverageGrade(ICollection<Grade> grades)
    {
        if (grades != null && grades.Count != 0)
        {
            return SubjectUtils.CalculateWeightedAverage(grades).ToString("0.##");
        }

        return "N/A";
    }

    #endregion

    #region Utility

    private int GetEducationSemester()
    {
        if (_currentEducationId == 0)
        {
            return 0;
        }

        var education = _educations.Find(e => e.Id.Equals(_currentEducationId));

        if (education is null)
        {
            return 0;
        }

        return education.Semesters;
    }

    private void SetSemesterFilterNumberQueryToValue(int value = 0)
    {
        var uri = new Uri(Navigation.Uri);
        var absoluteUri = uri.AbsoluteUri;

        const string pattern = @"(semesterFilterNumber=)(\d+)";

        if (value == 0)
        {
            // Remove the parameter if it exists
            absoluteUri = Regex.Replace(absoluteUri, pattern, match =>
            {
                // If it's the only parameter or the first one, clean up trailing "&" or "?"
                return match.Groups[1].Value == "?" ? "?" : "";
            });

            // Remove dangling "?" if it's the last character
            absoluteUri = absoluteUri.TrimEnd('?');
            absoluteUri = absoluteUri.TrimEnd('&');

            Navigation.NavigateTo(absoluteUri, forceLoad: false);
            return;
        }

        if (Regex.IsMatch(absoluteUri, pattern))
        {
            // Use a MatchEvaluator lambda to preserve regex capture groups correctly
            absoluteUri = Regex.Replace(absoluteUri, pattern, match => $"{match.Groups[1].Value}{value}");
        }
        else
        {
            // Add the parameter (with correct ? or & handling)
            var separator = absoluteUri.Contains('?') ? "&" : "?";
            absoluteUri += $"{separator}semesterFilterNumber={value}";
        }

        //var updatedUri = QueryHelpers.AddQueryString(uri.AbsoluteUri, "semesterFilterNumber", value.ToString());
        Navigation.NavigateTo(absoluteUri, forceLoad: false);
    }

    private int GetTotalGradeCountOfEducation()
    {
        return _currentEducationId == 0 ? 0 : _subjects.Sum(subject => subject.Grades.Count);
    }

    #endregion

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("removePageKeybinds", "HomePage");
        _objRef?.Dispose();
    }
}