using Microsoft.EntityFrameworkCore;

using TaxDashboard.Models;
using TaxDashboard.Models.Entities;

namespace TaxDashboard.Services;

public class IncomesService(IDbContextFactory<AppDbContext> contextFactory) : CrudService<Income>(contextFactory)
{
    public override async Task<Income?> GetDetailsAsync(int id)
    {
        using AppDbContext context = await ContextFactory.CreateDbContextAsync();
        return context.Incomes.Find(id);
    }

    public async Task<List<Income>> GetIncomesForYear(int clientId, int year)
    {
        using AppDbContext context = await ContextFactory.CreateDbContextAsync();

        IEnumerable<Income> existingIncomes = context.Incomes
            .AsNoTracking()
            .Include(i => i.Client)
            .Where(i => i.Client.Id == clientId && i.Date.Year == year);

        IEnumerable<Income> emptyIncomes = Enumerable.Range(1, 12).Select(month => new Income()
        {
            Amount = 0,
            Date = new(year, month, 1)
        });

        return [..existingIncomes
            .Union(emptyIncomes)
            .OrderBy(i => i.Date.Month)];
    }

    public override async Task UpdateAsync(Income entity)
    {
        using AppDbContext context = ContextFactory.CreateDbContext();
        context.Incomes.Update(entity);
        await context.SaveChangesAsync();
    }
}