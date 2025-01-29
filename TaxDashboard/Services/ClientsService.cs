using Microsoft.EntityFrameworkCore;

using TaxDashboard.Models;
using TaxDashboard.Models.Entities;

namespace TaxDashboard.Services;

public class ClientsService(IDbContextFactory<AppDbContext> contextFactory) : CrudService<Client>(contextFactory)
{
    public async Task<ICollection<Client>> GetAllWithDetailsAsync()
    {
        using AppDbContext context = await ContextFactory.CreateDbContextAsync();
#pragma warning disable IDE0305 // Simplify collection initialization
        return context.Clients
            .Include(c => c.Bank)
            .ToList();
#pragma warning restore IDE0305 // Simplify collection initialization
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

    public async Task<Client?> GetFirstWithDetailsAsync()
    {
        using AppDbContext context = await ContextFactory.CreateDbContextAsync();
        return context.Clients
            .Include(c => c.Bank)
            .Include(c => c.Incomes)
            .Include(c => c.JPKV7HandledDates)
            .Include(c => c.VATUEHandledDates)
            .Include(c => c.Settlements)
            .Include(c => c.Invoices)
            .FirstOrDefault();
    }

    public override async Task UpdateAsync(Client entity)
    {
        using AppDbContext context = ContextFactory.CreateDbContext();
        context.Clients.Update(entity);
        await context.SaveChangesAsync();
    }
}