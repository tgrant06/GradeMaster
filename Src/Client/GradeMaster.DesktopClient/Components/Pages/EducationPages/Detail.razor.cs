using BlazorBootstrap;
using GradeMaster.Client.Shared.Utility;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components;
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

    private decimal _educationAverage;

    protected async override Task OnInitializedAsync()
    {
        // Load the education details
        Education = await _educationRepository.GetByIdAsync(Id);

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

    #endregion

    #region Averages

    private async Task CalculateEducationAverage()
    {
        // var grades
        //await _gradeRepository.GetBySubjectIdsAsync(Education.Subjects.Select(s => s.Id).ToList());
        await _subjectRepository.GetByEducationIdAsync(Education.Id);
        _educationAverage = EducationUtils.CalculateEducationAverage(Education.Subjects);
    }

    #endregion
}