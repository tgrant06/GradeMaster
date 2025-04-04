using BlazorBootstrap;
using GradeMaster.Client.Shared.Utility;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;

namespace GradeMaster.DesktopClient.Components.Pages.EducationPages;

public partial class Detail
{
    #region Fields / Properties

    [Parameter]
    public int Id
    {
        get; set;
    }

    public Education Education
    {
        get; set;
    }

    public List<Subject> Subjects
    {
        get; set;
    }
    private bool IsExpanded { get; set; } = false;
    private bool IsTruncated { get; set; } = false;

    private string ButtonText => IsExpanded ? "less" : "more";
    //private string DescriptionClass => IsExpanded ? "expanded" : "collapsed";

    private string DescriptionAreaDynamicHeight => IsExpanded ? $"max-height: {_descriptionAreaExpandedHeight}px;" : "max-height: 175px;";

    private int _descriptionAreaExpandedHeight;

    private decimal _educationAverage;

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
        Education = await _educationRepository.GetByIdAsync(Id);
        Subjects = await _subjectRepository.GetByEducationIdOrderedAsync(Education.Id);

        // Calculate the average only after loading the data
        CalculateEducationAverage();
    }

    #region Description

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            IsTruncated = await JSRuntime.InvokeAsync<bool>("checkDescriptionHeight", "description-area", 175);
            _descriptionAreaExpandedHeight = await JSRuntime.InvokeAsync<int>("getMaxDescriptionHeight", "description-text");
            StateHasChanged();
        }
    }

    private void ToggleDescription()
    {
        IsExpanded = !IsExpanded;
    }

    #endregion

    #region Not used

    //invoke js function to scroll to top
    //protected async override Task OnAfterRenderAsync(bool firstRender)
    //{
    //    if (firstRender)
    //    {
    //        await JSRuntime.InvokeVoidAsync("scrollToTop");
    //    }
    //}

    #endregion

    private async Task RefreshSubjectData()
    {
        await _virtualizeComponent?.RefreshDataAsync();
    }

    #region Navigation

    private async Task GoBack() => await JSRuntime.InvokeVoidAsync("window.history.back");

    private void EditEducation() => Navigation.NavigateTo($"/educations/{Education.Id}/edit");

    private void GoToNewSubject(int educationId) => Navigation.NavigateTo($"/subjects/create?educationId={educationId}");

    #endregion

    #region Averages

    private void CalculateEducationAverage()
    {
        // Maybe if needed add more logic here
        _educationAverage = EducationUtils.CalculateEducationAverage(Subjects);
    }

    #endregion
}