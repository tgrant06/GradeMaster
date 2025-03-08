using BlazorBootstrap;
using GradeMaster.Client.Shared.Utility;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GradeMaster.DesktopClient.Components.Pages.SubjectPages;

public partial class Detail
{
    #region Fields / Properties

    [Parameter]
    public int Id
    {
        get; set;
    }

    public Subject Subject
    {
        get; set;
    }

    public List<Grade> Grades
    {
        get; set;
    }

    private decimal _subjectAverage;

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
        await _weightRepository.GetAllAsync();
        Subject = await _subjectRepository.GetByIdDetailAsync(Id);
        Grades = await _gradeRepository.GetBySubjectIdAsync(Subject.Id);

        // Calculate the average only after loading the education data
        CalculateSubjectAverage();
    }

    #region Navigation

    private async Task GoBack() => await JSRuntime.InvokeVoidAsync("window.history.back");

    private void EditSubject() => Navigation.NavigateTo($"/subjects/{Subject.Id}/edit");

    private void GoToEducation() => Navigation.NavigateTo($"/educations/{Subject.Education.Id}");

    private void GoToNewGrade(int subjectId) => Navigation.NavigateTo($"/grades/create?subjectId={subjectId}");

    #endregion

    #region Averages

    private void CalculateSubjectAverage()
    {
        // Maybe if needed add more logic here
        _subjectAverage = SubjectUtils.CalculateWeightedAverage(Grades);
    }

    #endregion
}