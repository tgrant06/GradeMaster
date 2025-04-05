using BlazorBootstrap;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GradeMaster.DesktopClient.Components.Pages.GradePages;

public partial class Detail : IAsyncDisposable
{
    #region Fields / Properties
    
    [Parameter]
    public int Id
    {
        get; set;
    }

    public Grade Grade
    {
        get; set;
    }

    private bool IsExpanded { get; set; } = false;
    private bool IsTruncated { get; set; } = false;

    private string ButtonText => IsExpanded ? "less" : "more";
    //private string DescriptionClass => IsExpanded ? "expanded" : "collapsed";

    private string DescriptionAreaDynamicHeight => IsExpanded ? $"max-height: {_descriptionAreaExpandedHeight}px;" : "max-height: 175px;";

    private int _descriptionAreaExpandedHeight;

    //private decimal _subjectAverage;

    #endregion

    #region Dependency Injection

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

    protected async override Task OnInitializedAsync()
    {
        Grade = await _gradeRepository.GetByIdDetailAsync(Id);

        // Calculate the average only after loading the data
        //await CalculateSubjectAverage();
    }

    #region Description

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            IsTruncated = await JSRuntime.InvokeAsync<bool>("checkDescriptionHeight", "description-area", 175);
            await SetDescriptionAreaExpandedHeight();
            StateHasChanged();
        }
    }

    private async Task ToggleDescription()
    {
        if (!IsExpanded)
        {
            await SetDescriptionAreaExpandedHeight();
        }

        IsExpanded = !IsExpanded;

        await DynamicDescriptionHeightActive(IsExpanded);
    }

    private async Task SetDescriptionAreaExpandedHeight()
    {
        _descriptionAreaExpandedHeight = await JSRuntime.InvokeAsync<int>("getMaxDescriptionHeight", "description-text");
    }

    private async Task DynamicDescriptionHeightActive(bool isActive)
    {
        if (isActive)
        {
            await JSRuntime.InvokeVoidAsync("addDescriptionAreaEventListener");
        }
        else
        {
            await JSRuntime.InvokeVoidAsync("removeDescriptionAreaEventListener");
        }
    }

    #endregion

    #region Navigation

    private async Task GoBack() => await JSRuntime.InvokeVoidAsync("window.history.back");

    private void EditGrade() => Navigation.NavigateTo($"/grades/{Grade.Id}/edit");

    private void GoToEducation() => Navigation.NavigateTo($"/educations/{Grade.Subject.Education.Id}");

    private void GoToSubject() => Navigation.NavigateTo($"/subjects/{Grade.Subject.Id}");

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

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("removeDescriptionAreaEventListener");
    }
}