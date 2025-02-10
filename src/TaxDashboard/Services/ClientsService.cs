using LifeManagers.Data.Services;

using Microsoft.EntityFrameworkCore;

using TaxDashboard.Data;
using TaxDashboard.Data.Entities;

namespace TaxDashboard.Services;

public class ClientsService(IDbContextFactory<AppDbContext> contextFactory) : CrudService<Client, AppDbContext>(contextFactory)
{
    public async Task<ICollection<Client>> GetAllWithDetailsAsync()
    {
        using AppDbContext context = await ContextFactory.CreateDbContextAsync();
        return [..context.Clients
            .Include(c => c.Bank)];
    }

    public override async Task<Client?> GetDetailsAsync(int id)
    {
        using AppDbContext context = await ContextFactory.CreateDbContextAsync();
        return context.Clients
            .Include(c => c.Bank)
            .Include(c => c.Incomes)
            .Include(c => c.JPKV7HandledDates)
            .Include(c => c.VATUEHandledDates)
            .Include(c => c.Settlements)
            .Include(c => c.Invoices)
            .FirstOrDefault(c => c.Id == id);
    }

    public async Task<Client?> GetFirstSelectableClientWithDetailsAsync()
    {
        using AppDbContext context = await ContextFactory.CreateDbContextAsync();
        return context.Clients
            .Include(c => c.Bank)
            .Include(c => c.Incomes)
            .Include(c => c.JPKV7HandledDates)
            .Include(c => c.VATUEHandledDates)
            .Include(c => c.Settlements)
            .Include(c => c.Invoices)
            .FirstOrDefault(c => !c.Suspended);
    }

    public async Task<decimal> GetIncomeSumAsync(int clientId, DateOnly startDate, DateOnly endDate)
    {
        using AppDbContext context = await ContextFactory.CreateDbContextAsync();
        Client? client = context.Clients
            .Include(c => c.Incomes)
            .FirstOrDefault(c => c.Id == clientId);

        return client is null
            ? 0
            : client.Incomes
                .Where(i => i.Date >= startDate && i.Date <= endDate)
                .Sum(i => i.Amount);
    }
}