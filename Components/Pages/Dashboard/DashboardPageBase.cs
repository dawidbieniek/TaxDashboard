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

    protected Client? Client { get; private set; }
    protected DateOnly? ContextDate { get; private set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);
    }

    protected async Task ChangeSelectedClient(Client client)
    {
        Client = client;
        await LocalStorage.SetItemAsync(StorageClientIdKey, client.Id);
    }

    protected async Task ChangeDateContext(DateOnly date)
    {
        ContextDate = date;
        await LocalStorage.SetItemAsync(StorageDateContextKey, date);
    }

    protected override async Task OnInitializedAsync()
    {
        int? storedClientId = await LocalStorage.GetItemAsync<int>(StorageClientIdKey);
        if (storedClientId.HasValue)
            Client = await ClientsService.GetAsync(storedClientId.Value);

        DateOnly? storedDateContext = await LocalStorage.GetItemAsync<DateOnly>(StorageDateContextKey);
        if (storedDateContext.HasValue)
            ContextDate = storedDateContext.Value;

        // HACK: Will I need to keep initialized set to end of childs overrides?

        Initialized = true;
        StateHasChanged();

        // _banks = await BanksService.GetAllAsync();
    }
}