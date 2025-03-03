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
    [Parameter]
    public int Id
    {
        get; set;
    }

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

    public Education Education
    {
        get; set;
    }

    public List<Subject> Subjects
    {
        get; set;
    }

    private decimal _educationAverage;

    private Virtualize<Subject>? _virtualizeComponent;

    private ConfirmDialog _dialog = default!;

    protected async override Task OnInitializedAsync()
    {
        // Load the education details
        await _weightRepository.GetAllAsync();

        Education = await _educationRepository.GetByIdAsync(Id);

        Subjects = await _subjectRepository.GetByEducationIdOrderedAsync(Education.Id);

        // Calculate the average only after loading the education data
        await CalculateEducationAverage();
    }
    //invoke js function to scroll to top
    //protected async override Task OnAfterRenderAsync(bool firstRender)
    //{
    //    if (firstRender)
    //    {
    //        await JSRuntime.InvokeVoidAsync("scrollToTop");
    //    }
    //}

    private async Task RefreshSubjectData()
    {
        await _virtualizeComponent?.RefreshDataAsync();
    }

    private string DescriptionString() => string.IsNullOrEmpty(Education.Description) ? "-" : Education.Description;

    #region Navigation

    private async Task GoBack()
    {
        await JSRuntime.InvokeVoidAsync("window.history.back");
    }

    private void EditEducation()
    {
        Navigation.NavigateTo($"/educations/{Education.Id}/edit");
    }

    private void GoToNewSubject(int educationId) => Navigation.NavigateTo($"/subjects/create?educationId={educationId}");

    #endregion

    #region Averages

    private async Task CalculateEducationAverage()
    {
        // var grades
        //await _gradeRepository.GetBySubjectIdsAsync(Education.Subjects.Select(s => s.Id).ToList());
        //await _subjectRepository.GetByEducationIdOrderedAsync(Education.Id);
        _educationAverage = EducationUtils.CalculateEducationAverage(Subjects);
    }

    #endregion
}