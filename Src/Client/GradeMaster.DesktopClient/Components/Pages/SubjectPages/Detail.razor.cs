using BlazorBootstrap;
using GradeMaster.Client.Shared.Utility;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GradeMaster.DesktopClient.Components.Pages.SubjectPages;

public partial class Detail
{
    [Parameter]
    public int Id
    {
        get; set;
    }

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

    public Subject Subject
    {
        get; set;
    }

    private decimal _subjectAverage;

    protected async override Task OnInitializedAsync()
    {
        // Load the education details
        Subject = await _subjectRepository.GetByIdDetailAsync(Id);
        // await _educationRepository.GetBySubjectIdAsync(Subject.Id);
        // Calculate the average only after loading the education data
        await CalculateSubjectAverage();
    }

    private string DescriptionString() => string.IsNullOrEmpty(Subject.Description) ? "-" : Subject.Description;

    #region Navigation

    private async Task GoBack()
    {
        await JSRuntime.InvokeVoidAsync("window.history.back");
    }

    private void EditSubject()
    {
        Navigation.NavigateTo($"/subjects/{Subject.Id}/edit");
    }

    private void GoToEducation()
    {
        Navigation.NavigateTo($"/educations/{Subject.Education.Id}");
    }

    #endregion

    #region Averages

    private async Task CalculateSubjectAverage()
    {
        var grades = await _gradeRepository.GetBySubjectIdAsync(Subject.Id);
        _subjectAverage = SubjectUtils.CalculateWeightedAverage(grades);
    }

    #endregion
}