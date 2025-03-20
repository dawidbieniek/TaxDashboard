using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

using MudBlazor;

namespace TaxDashboard.Components.Dialogs;

public partial class UnsavedContextChecker : ComponentBase, IDisposable
{
    private IDialogReference? _dialog;
    private IDisposable? _navigationLockRegistration;

    private string? _lastNavigationTarget;
    private bool _modalShowing = false;
    private bool _ignoreChanges = false;

    [Inject]
    public IDialogService DialogService { get; set; } = default!;

    [Inject]
    public NavigationManager Navigation { get; set; } = default!;

    [Parameter, EditorRequired]
    public Func<bool> NavigationBlockCondition { get; set; } = () => false;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
            _navigationLockRegistration = Navigation.RegisterLocationChangingHandler(OnLocationChanging);
    }

    private async ValueTask OnLocationChanging(LocationChangingContext context)
    {
        if (context.TargetLocation == Navigation.Uri)
            return;

        if (!_ignoreChanges && NavigationBlockCondition())
        {
            _lastNavigationTarget = context.TargetLocation;

            if (!_modalShowing) // Prevents showing multiple modals at once
                await ShowAsync();

            context.PreventNavigation();
        }
        else if (_modalShowing)
            Hide();
    }

    private void ForceNavigation()
    {
        if (_lastNavigationTarget is null)
            return;

        _ignoreChanges = true;
        Navigation.NavigateTo(_lastNavigationTarget);
    }

    private async Task ShowAsync()
    {
        _modalShowing = true;
        _dialog = await DialogService.ShowAsync<UnsavedChangesDialog>("Unsaved changes", parameters: new()
        {
            { "OnDiscardChanges", EventCallback.Factory.Create(this, ForceNavigation)}
        });

        DialogResult? result = await _dialog.Result;
        if (result?.Canceled ?? true)
            Hide();
    }

    private void Hide()
    {
        _modalShowing = false;
        _ignoreChanges = false;
        _dialog?.Close(DialogResult.Cancel());
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Hide();
        _navigationLockRegistration?.Dispose();
    }
}