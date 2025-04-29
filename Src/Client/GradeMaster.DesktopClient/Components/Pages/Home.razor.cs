using BlazorBootstrap;
using GradeMaster.Client.Shared.Utility;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;

namespace GradeMaster.DesktopClient.Components.Pages;

public partial class Home : IAsyncDisposable
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

    private DotNetObjectReference<Home>? _objRef;

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

        _objRef = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("addPageKeybinds", "HomePage", _objRef);

        var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("educationId", out var educationIdString) && int.TryParse(educationIdString, out var educationId))
        {
            var educationExists = await _educationRepository.ExistsAsync(educationId);

            if (educationExists)
            {
                _selectedEducationId = educationId;
                await LoadEducationData(educationId);
                return;
            }

            ToastService.Notify(new ToastMessage(ToastType.Info, $"This education does no longer exist."));
        }

        // Navigate to the URL with the default query parameter
        Navigation.NavigateTo($"?", false);
    }

    #region Data

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

    #endregion

    #region Navigation

    private void GoToSubject(int subjectId) => Navigation.NavigateTo($"/subjects/{subjectId}");

    private void GoToGrade(int gradeId) => Navigation.NavigateTo($"/grades/{gradeId}");

    private void GoToEducation(int educationId) => Navigation.NavigateTo($"/educations/{educationId}");

    private void GoToNewSubject(int educationId) => Navigation.NavigateTo($"/subjects/create?educationId={educationId}");

    private void GoToNewGrade(int subjectId) => Navigation.NavigateTo($"/grades/create?subjectId={subjectId}");

    private void GoToNewGradeFromEducationId(int educationId) => Navigation.NavigateTo($"/grades/create?educationId={educationId}");

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
            ToastService.Notify(new ToastMessage(ToastType.Info, $"Select Education first"));
            return;
        }

        if (_educations.Find(e => e.Id.Equals(_currentEducationId))!.Completed)
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, $"Education is completed"));
            return;
        }

        GoToNewSubject(_currentEducationId);
    }

    [JSInvokable]
    public async Task NavigateToGradeCreate()
    {
        if (_currentEducationId == 0)
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, $"Select Education first"));
            return;
        }

        var educationHasInProgressSubjects =
            await _subjectRepository.ExistsAnyIsCompletedWithEducationIdAsync(_currentEducationId, false);

        if (!educationHasInProgressSubjects)
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, $"No 'In Progress' subjects"));
            return;
        }

        GoToNewGradeFromEducationId(_currentEducationId);
    }

    [JSInvokable]
    public void NavigateToEducationDetail()
    {
        if (_currentEducationId == 0)
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, $"Select Education first"));
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

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("removePageKeybinds", "HomePage");
        _objRef?.Dispose();
    }
}