using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;

namespace TaxDashboard.Components.Popups;
public class PreventNavigationModal : Modal
{
    public string _message = "Zmiany nie zostały zapisane. Opuszczenie strony spowoduje ich utratę!";
    private string? _lastNavigationTarget;
    private bool _ignoreChanges = false;

    public PreventNavigationModal()
    {
        Title = "Niezapisane zmiany";
        ChildContent = builder =>
        {
            builder.AddContent(0, _message);
        };
        ButtonText = "Odrzuć zmiany";
        ButtonClick = EventCallback.Factory.Create(this, ForceNavigation);
        BlockNavigationWhenShowing = true;
    }

    [Inject]
    public NavigationManager Navigation { get; set; } = default!;

    [Parameter, EditorRequired]
    public Func<bool> NavigationBlockCondition { get; set; } = () => false;

    protected override async ValueTask OnLocationChanging(LocationChangingContext context)
    {
        if (context.TargetLocation == Navigation.Uri)
            return;

        if (!_ignoreChanges && NavigationBlockCondition())
        {
            _lastNavigationTarget = context.TargetLocation;

            if (!ModalShowing) // Prevents showing multiple modals at once
                await ShowAsync();

            context.PreventNavigation();
        }
        else if (ModalShowing)
            await HideAsync();
    }

    private void ForceNavigation()
    {
        if (_lastNavigationTarget is null)
            return;

        _ignoreChanges = true;
        Navigation.NavigateTo(_lastNavigationTarget);
    }
}