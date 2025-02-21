using LifeManagers.Data.Services;

using Microsoft.EntityFrameworkCore;

using TaxDashboard.Data;
using TaxDashboard.Data.Entities;
using TaxDashboard.Util;

namespace TaxDashboard.Services;

public class ClientsService(IDbContextFactory<AppDbContext> contextFactory) : CrudService<Client, AppDbContext>(contextFactory)
{
    public const int ZusPlusLimit = 120_000;
    public const int BaseVatLimit = 200_000;
    public const int FiscalLimit = 20_000;

    public async Task<ICollection<Client>> GetAllWithDetailsAsync()
    {
        using AppDbContext context = await ContextFactory.CreateDbContextAsync();
        return [..context.Clients
            .Include(c => c.Bank)
            .Include(c => c.Incomes)];
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
            .AsSplitQuery()
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
            .AsSplitQuery()
            .FirstOrDefault(c => !c.Suspended);
    }

    /// <summary>
    /// Returns sum of client incomes in given vat year. 
    /// </summary>
    /// <remarks>Client needs to have Income field populated!</remarks>
    public static decimal GetVatIncomeSum(Client client, DateOnly contextDate)
    {
        DateOnly startDate = client.JoinDateTime.Year == contextDate.Year
            ? DateOnly.FromDateTime(client.JoinDateTime).FirstOfCurrentMonth()
            : contextDate.FirstDayOfCurrentYear();

        return client.Incomes
            .Where(i => i.Date >= startDate && i.Date <= contextDate.LastDayOfCurrentYear())
            .Sum(i => i.Amount);
    }

    /// <summary>
    /// Returns sum of client incomes in given fiscal year. 
    /// </summary>
    /// <remarks>Client needs to have Income field populated!</remarks>
    public static decimal GetFiscalIncomeSum(Client client, DateOnly contextDate)
    {
        DateOnly startDate = client.FirstCashRegisterUseDate.Year == contextDate.Year
            ? client.FirstCashRegisterUseDate.FirstOfCurrentMonth()
            : contextDate.FirstDayOfCurrentYear();

        return client.Incomes
            .Where(i => i.Date >= startDate && i.Date <= contextDate.LastDayOfCurrentYear())
            .Sum(i => i.Amount);
    }

    /// <summary>
    /// Returns sum of client incomes in given zus year. 
    /// </summary>
    /// <remarks>Client needs to have Income field populated!</remarks>
    public static decimal GetZusIncomeSum(Client client, DateOnly contextDate)
    {
        return client is null || client.ReductionChangeDate == default
            ? 0
            : client.Incomes
                .Where(i => i.Date >= contextDate.FirstDayOfCurrentYear() && i.Date <= contextDate.LastDayOfCurrentYear())
                .Sum(i => i.Amount);
    }

    public static decimal GetVatLimit(Client client, DateOnly contextDate)
    {
        return client.JoinDateTime.Year == contextDate.Year
            ? Math.Round((decimal)BaseVatLimit * client.JoinDateTime.DaysUntilEndOfYear() / new System.Globalization.GregorianCalendar().GetDaysInYear(contextDate.Year), 2)
            : BaseVatLimit;
    }
}