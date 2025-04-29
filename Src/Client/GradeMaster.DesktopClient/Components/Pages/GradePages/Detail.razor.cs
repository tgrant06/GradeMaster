using BlazorBootstrap;
using GradeMaster.Client.Shared.Utility;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using GradeMaster.DataAccess.Repositories;
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

    public Grade Grade { get; set; } = new();

    private bool IsExpanded { get; set; } = false;

    private bool IsTruncated { get; set; } = false;

    private string ButtonText => IsExpanded ? "less" : "...more";

    private string DescriptionAreaDynamicHeight => IsExpanded ? $"max-height: {_descriptionAreaExpandedHeight}px;" : "max-height: 175px;";

    private int _descriptionAreaExpandedHeight;

    private ConfirmDialog _dialog = default!;

    private DotNetObjectReference<Detail>? _objRef;

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
        // Initialize to avoid null reference
        Grade.Subject = new Subject { Education = new Education() };
        Grade.Weight = new Weight();

        var educationExists = await _gradeRepository.ExistsAsync(Id);

        if (!educationExists)
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, $"This grade does no longer exist."));
            await GoBack();
            return;
        }

        _objRef = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("addPageKeybinds", "GradeDetailPage", _objRef);

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

    #region Delete Grade

    private async Task DeleteGradeAsync()
    {
        var options = new ConfirmDialogOptions
        {
            YesButtonColor = ButtonColor.Danger,
        };

        var confirmation = await _dialog.ShowAsync(
            title: "Are you sure you want to delete this Grade?",
            message1: $"Grade from Subject: {Grade.Subject.Name} - {Grade.Subject.Semester} and Education: {Grade.Subject.Education.Name} with Value: {Grade.Value}, Weight: {Grade.Weight.Name}, Date: {Grade.Date.ToShortDateString()} and Description: {UIUtils.TruncateString(Grade.Description ?? "-", 35)} will be deleted.",
            message2: "Do you want to proceed?",
            confirmDialogOptions: options);

        if (confirmation)
        {
            try
            {
                await _gradeRepository.DeleteByIdAsync(Grade.Id);
                await GoBack(); // was await OnGradeDeleted.InvokeAsync(Grade.Id);
                ToastService.Notify(new ToastMessage(ToastType.Success, $"Grade deleted successfully.")); // maybe add Name of deleted object
            }
            catch (Exception e)
            {
                ToastService.Notify(new ToastMessage(ToastType.Danger, $"Error deleting grade: {e.Message}"));
            }
        }
    }

    #endregion

    #region Navigation

    private async Task GoBack() => await JSRuntime.InvokeVoidAsync("window.history.back");

    private void EditGrade() => Navigation.NavigateTo($"/grades/{Grade.Id}/edit");

    private void GoToEducation() => Navigation.NavigateTo($"/educations/{Grade.Subject.Education.Id}");

    private void GoToSubject() => Navigation.NavigateTo($"/subjects/{Grade.Subject.Id}");

    #endregion

    #region JSInvokable / Keybinds

    [JSInvokable]
    public void NavigateToEdit() => EditGrade();

    [JSInvokable]
    public async Task DeleteObject() => await DeleteGradeAsync();

    [JSInvokable]
    public void NavigateToEducation() => GoToEducation();

    [JSInvokable]
    public void NavigateToSubject() => GoToSubject();

    [JSInvokable]
    public async Task ToggleDescriptionHeight()
    {
        if (IsTruncated)
        {
            await ToggleDescription();
            StateHasChanged();
        }
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

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("removeDescriptionAreaEventListener");

        await JSRuntime.InvokeVoidAsync("removePageKeybinds", "GradeDetailPage");
        _objRef?.Dispose();
    }
}