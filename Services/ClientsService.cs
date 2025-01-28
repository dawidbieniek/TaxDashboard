using Microsoft.EntityFrameworkCore;

using TaxDashboard.Models;
using TaxDashboard.Models.Entities;

namespace TaxDashboard.Services;

public class ClientsService(IDbContextFactory<AppDbContext> contextFactory) : CrudService<Client>(contextFactory)
{
    public override async Task<Client?> GetDetailsAsync(int id)
    {
        using AppDbContext context = await ContextFactory.CreateDbContextAsync();
        return context.Clients.Find(id);
    }
}