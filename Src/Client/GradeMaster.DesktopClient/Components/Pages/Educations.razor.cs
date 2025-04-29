using BlazorBootstrap;
using GradeMaster.Common.Entities;
using GradeMaster.DataAccess.Interfaces.IRepositories;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;

namespace GradeMaster.DesktopClient.Components.Pages;

public partial class Educations : IAsyncDisposable
{
    #region Fields / Properties

    private string _searchValue = string.Empty;

    private Virtualize<Education>? _virtualizeComponent;

    private ConfirmDialog _dialog = default!;

    private DotNetObjectReference<Educations>? _objRef;

    #endregion

    #region Dependency Injection

    [Inject]
    private IEducationRepository _educationRepository
    {
        get; set;
    }

    [Inject]
    private IWeightRepository _weightRepository
    {
        get; set;
    }

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
        _objRef = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("addPageKeybinds", "EducationsPage", _objRef);

        await _weightRepository.GetAllAsync();

        var uri = new Uri(Navigation.Uri);
        var queryParams = QueryHelpers.ParseQuery(uri.Query);
        if (queryParams.TryGetValue("q", out var searchValue))
        {
            var searchValueString = searchValue.ToString();
            _searchValue = string.IsNullOrWhiteSpace(searchValueString) ? string.Empty : searchValueString;
        }
    }

    #region Data

    private async ValueTask<ItemsProviderResult<Education>> GetEducationsProvider(ItemsProviderRequest request)
    {
        var startIndex = request.StartIndex;
        var count = request.Count;

        // Fetch only the required slice of data
        var fetchedEducations = await _educationRepository.GetBySearchWithRangeAsync(_searchValue, startIndex, count);

        // Calculate the total number of items (if known or needed)
        var totalItemCount = await _educationRepository.GetTotalCountAsync(_searchValue);

        // Return the result to the Virtualize component
        return new ItemsProviderResult<Education>(fetchedEducations, totalItemCount);
    }

    private async Task RefreshEducationData()
    {
        var uri = new Uri(Navigation.Uri);
        var baseUri = uri.GetLeftPart(UriPartial.Path);
        var updatedUri = QueryHelpers.AddQueryString(baseUri, "q", _searchValue);
        Navigation.NavigateTo(updatedUri, forceLoad: false);

        await _virtualizeComponent?.RefreshDataAsync()!;
    }

    private async Task LoadAllEducations()
    {
        _searchValue = string.Empty;
        await RefreshEducationData();
    }

    #endregion

    #region Not Used

    // protected override async Task OnAfterRenderAsync(bool firstRender)
    // {
    //     if (firstRender)
    //     {
    //         await JSRuntime.InvokeVoidAsync("initializeGridAnimation");
    //     }
    // }

    // private async Task LoadEducations()
    // {
    //     _educations = await _educationRepository.GetAllAsync();
    //     _filteredEducations = _educations;
    // }

    // private async Task FilterEducations()
    // {
    //     await _virtualizeComponent?.RefreshDataAsync();

    //     if (string.IsNullOrWhiteSpace(_searchValue))
    //     {
    //         _filteredEducations = _educations;
    //     }
    //     else
    //     {
    //         _filteredEducations = _educations
    //             .Where(education =>
    //                 education.Name.Contains(_searchValue, StringComparison.OrdinalIgnoreCase) ||
    //                 (education.Description != null && education.Description.Contains(_searchValue, StringComparison.OrdinalIgnoreCase)) ||
    //                 education.Semesters.ToString().Contains(_searchValue, StringComparison.OrdinalIgnoreCase) ||
    //                 (education.Institution != null && education.Institution.Contains(_searchValue, StringComparison.OrdinalIgnoreCase)))
    //             .ToList();
    //     }
    // }

    // was void HandleEducationDeleted(int educationId)
    // private async Task HandleEducationDeleted()
    // {
    //     was void HandleEducationDeleted(int educationId)
    //     _virtualizeComponent?.RefreshDataAsync(); causes some sort of error in the dev console in bootstrap js file 
    //     error does not affect the application, but it is annoying

    //     var education = _filteredEducations.FirstOrDefault(e => e.Id == educationId);
    //     if (education != null)
    //     {
    //         _filteredEducations.Remove(education);
    //     }

    //     Trigger a refresh of the Virtualize component
    //     await _virtualizeComponent?.RefreshDataAsync();
    // }

    #endregion

    #region Navigation

    private void CreateEducation() => Navigation.NavigateTo("/educations/create");

    #endregion

    #region JSInvokable / Keybinds

    [JSInvokable]
    public void NavigateToCreate() => CreateEducation();

    [JSInvokable]
    public async Task ClearSearch()
    {
        await LoadAllEducations();
        StateHasChanged();
    }

    #endregion

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("removePageKeybinds", "EducationsPage");
        _objRef?.Dispose();
    }
}