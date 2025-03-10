using BlazorBootstrap;
using GradeMaster.Common.Enums;
using GradeMaster.Logic.Interfaces.IServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using Entities = GradeMaster.Common.Entities;

namespace GradeMaster.Client.Shared.Components.Education;

public partial class EducationForm
{
    #region Fields / Properties

    [Parameter]
    public FormType FormType
    {
        get; set;
    }

    [Parameter]
    public Entities.Education? Education
    {
        get; set;
    }

    [Parameter]
    public EventCallback<Entities.Education> OnSave
    {
        get; set;
    }

    private string FormTitle { get; set; } = "Education Form";

    public Entities.Education NewEducation { get; set; } = new();

    private EditContext? _editContext;

    #endregion

    #region Dependency Injection

    [Inject]
    private IJSRuntime JSRuntime
    {
        get; set;
    }

    [Inject]
    private NavigationManager Navigation
    {
        get; set;
    }

    [Inject]
    private IEducationService _educationService
    {
        get; set;
    }

    [Inject] protected ToastService ToastService { get; set; } = default!;

    #endregion

    protected override void OnInitialized()
    {
        if (Education == null)
        {
            // Initialize Education for creation
            Education = new Entities.Education
            {
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddYears(1)),
                Semesters = 2,
                Completed = false
            };

            FormTitle = "Create New Education";
        }
        else
        {
            FormTitle = "Edit Education";
        }
        _educationService.PassObjectAttributes(NewEducation, Education);

        _editContext = new EditContext(NewEducation);
    }

    #region HandleSubmit

    private async Task HandleValidSubmit()
    {
        if (NewEducation.EndDate < NewEducation.StartDate)
        {
            ToastService.Notify(new ToastMessage(ToastType.Warning, $"End Date cannot be before Start Date."));
            return;
        }

        if (OnSave.HasDelegate)
        {
            _educationService.PassObjectAttributes(Education, NewEducation, true);
            await OnSave.InvokeAsync(Education);
        }
        else
        {
            // Optional fallback logic
            // show error?
            ToastService.Notify(new ToastMessage(ToastType.Danger, $"Education could not be saved."));
        }
    }

    private void HandleInvalidSubmit()
    {
        ToastService.Notify(new ToastMessage(ToastType.Warning, $"Education form is not valid."));
    }

    #endregion


    private async Task Cancel() => await JSRuntime.InvokeVoidAsync("window.history.back");
}