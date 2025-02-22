using BlazorBootstrap;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GradeMaster.DesktopClient.Components.Pages.GradePages;

public partial class Detail
{
    [Parameter]
    public int Id
    {
        get; set;
    }

    #region Dependancy Injection

    [Inject]
    private IGradeRepository _gradeRepository
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

    public Grade Grade
    {
        get; set;
    }

    //private decimal _subjectAverage;

    protected async override Task OnInitializedAsync()
    {
        // Load the education details
        Grade = await _gradeRepository.GetByIdDetailAsync(Id);

        // Calculate the average only after loading the education data
        //await CalculateSubjectAverage();
    }

    private string DescriptionString() => string.IsNullOrEmpty(Grade.Description) ? "-" : Grade.Description;

    #region Navigation

    private async Task GoBack()
    {
        await JSRuntime.InvokeVoidAsync("window.history.back");
    }

    private void EditGrade()
    {
        Navigation.NavigateTo($"/grades/{Grade.Id}/edit");
    }

    private void GoToEducation()
    {
        Navigation.NavigateTo($"/educations/{Grade.Subject.Education.Id}");
    }

    private void GoToSubject()
    {
        Navigation.NavigateTo($"/subjects/{Grade.Subject.Id}");
    }

    #endregion

    #region Averages Currently Not Used

    // private async Task CalculateSubjectAverage()
    // {
    //     var grades = await _gradeRepository.GetBySubjectIdAsync(Grade.Id);
    //     //_subjectAverage = CalculateWeightedAverage(grades);
    // }

    // private decimal CalculateWeightedAverage(ICollection<Grade> grades)
    // {
    //     if (grades == null || !grades.Any())
    //     {
    //         return 0;
    //     }

    //     decimal totalWeight = grades.Sum(g => g.Weight?.Value ?? 1); // Default weight to 1 if null
    //     if (totalWeight == 0)
    //     {
    //         return 0;
    //     }

    //     decimal weightedSum = grades.Sum(g => g.Value * (g.Weight?.Value ?? 1)); // Default weight to 1 if null
    //     return weightedSum / totalWeight;
    // }

    #endregion

}