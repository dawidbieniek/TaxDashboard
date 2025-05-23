﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

using TaxDashboard.Components.Navbar;
using TaxDashboard.Data.Entities;
using TaxDashboard.Services;

namespace TaxDashboard.Components.Pages.Dashboard;

public class DashboardPageBase : ComponentBase
{
    [Inject]
    public ClientsService ClientsService { get; set; } = default!;

    protected bool Initialized { get; private set; } = false;

    protected virtual Client? Client { get; set; }
    protected ContextDate? ContextDate { get; private set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);
    }

    protected virtual async Task ChangeSelectedClient(Client client)
    {
        Client = await ClientsService.GetDetailsAsync(client.Id);
        Preferences.Set(GlobalSettings.PreferencesStorage.LastClientIdKey, client.Id);
    }

    protected virtual Task ChangeDateContext(ContextDate date)
    {
        ContextDate = date;
        Preferences.Set(GlobalSettings.PreferencesStorage.LastDateContextKey, date.Date.ToString(GlobalSettings.PreferencesStorage.DateStorageFormat));
        return Task.CompletedTask;
    }

    protected override async Task OnInitializedAsync()
    {
        int storedClientId = Preferences.Get(GlobalSettings.PreferencesStorage.LastClientIdKey, -1);
        if (storedClientId > 0)
        {
            Client? storedClient = await ClientsService.GetDetailsAsync(storedClientId);
            if (storedClient is not null && !storedClient.Suspended)
                Client = storedClient;
        }

        Client ??= await ClientsService.GetFirstSelectableClientWithDetailsAsync();

        if (DateOnly.TryParseExact(Preferences.Get(GlobalSettings.PreferencesStorage.LastDateContextKey, null), GlobalSettings.PreferencesStorage.DateStorageFormat, out DateOnly storedDateContext))
            ContextDate = new(storedDateContext, false);

        await OnAfterRequiredInitializedAsync();

        Initialized = true;
        StateHasChanged();
    }

    protected virtual Task OnAfterRequiredInitializedAsync() => Task.CompletedTask;
}