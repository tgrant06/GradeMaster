using BlazorBootstrap;
using GradeMaster.Client.Shared.Utility;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
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

    private bool IsExpanded { get; set; } = false;
    private bool IsTruncated { get; set; } = false;

    private string ButtonText => IsExpanded ? "less" : "more";
    //private string DescriptionClass => IsExpanded ? "expanded" : "collapsed";

    private string DescriptionAreaDynamicHeight => IsExpanded ? $"max-height: {_descriptionAreaExpandedHeight}px;" : "max-height: 175px;";

    private int _descriptionAreaExpandedHeight;

    private decimal _subjectAverage;

    private Virtualize<Grade>? _virtualizeComponent;

    private ConfirmDialog _dialog = default!;

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

    private async Task RefreshGradeData()
    {
        await _virtualizeComponent?.RefreshDataAsync();
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

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("removeDescriptionAreaEventListener");
    }
}