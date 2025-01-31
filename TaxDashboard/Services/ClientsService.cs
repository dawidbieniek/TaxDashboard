using Microsoft.EntityFrameworkCore;

using TaxDashboard.Models;
using TaxDashboard.Models.Entities;

namespace TaxDashboard.Services;

public class ClientsService(IDbContextFactory<AppDbContext> contextFactory) : CrudService<Client>(contextFactory)
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

    public override async Task UpdateAsync(Client entity)
    {
        using AppDbContext context = ContextFactory.CreateDbContext();
        context.Clients.Update(entity);
        await context.SaveChangesAsync();
    }
}