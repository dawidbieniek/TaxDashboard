using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

using TaxDashboard.Models.Entities;
using TaxDashboard.Services;

namespace TaxDashboard.Components.Pages.Dashboard;

public class DashboardPageBase : ComponentBase
{
    protected const string StorageClientIdKey = "ClientId";
    protected const string StorageDateContextKey = "DateContext";

    [Inject]
    public ILocalStorageService LocalStorage { get; set; } = default!;

    [Inject]
    public ClientsService ClientsService { get; set; } = default!;

    protected bool Initialized { get; private set; } = false;

    protected virtual Client? Client { get; set; }
    protected DateOnly? ContextDate { get; private set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);
    }

    protected virtual async Task ChangeSelectedClient(Client client)
    {
        Client = await ClientsService.GetDetailsAsync(client.Id);
        await LocalStorage.SetItemAsync(StorageClientIdKey, client.Id);
    }

    protected virtual async Task ChangeDateContext(DateOnly date)
    {
        ContextDate = date;
        await LocalStorage.SetItemAsync(StorageDateContextKey, date);
    }

    protected override async Task OnInitializedAsync()
    {
        int? storedClientId = await LocalStorage.GetItemAsync<int>(StorageClientIdKey);
        if (storedClientId.HasValue)
            Client = await ClientsService.GetDetailsAsync(storedClientId.Value);

        Client ??= await ClientsService.GetFirstWithDetailsAsync();

        DateOnly? storedDateContext = await LocalStorage.GetItemAsync<DateOnly>(StorageDateContextKey);
        if (storedDateContext.HasValue)
            ContextDate = storedDateContext.Value;

        OnAfterRequiredInitialized();

        Initialized = true;
        StateHasChanged();
    }

    protected virtual void OnAfterRequiredInitialized()
    { }
}