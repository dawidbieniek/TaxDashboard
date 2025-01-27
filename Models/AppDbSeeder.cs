using TaxDashboard.Models.Entities;

namespace TaxDashboard.Models;

public static class AppDbSeeder
{
    private static readonly IList<Bank> Banks =
    [
        new() { Name = "Aion Bank" },
        new() { Name = "Alior Bank" },
        new() { Name = "Bank BNP Paribas" },
        new() { Name = "Bank BPH S.A." },
        new() { Name = "Bank BPS" },
        new() { Name = "Bank Millennium" },
        new() { Name = "Bank Nowy" },
        new() { Name = "Bank Pocztowy" },
        new() { Name = "Bank Pekao S.A." },
        new() { Name = "Bank Śląski" },
        new() { Name = "BOŚ Bank" },
        new() { Name = "Citi Bank Handlowy" },
        new() { Name = "Credit Agricole" },
        new() { Name = "Deutsche Bank Polska S.A." },
        new() { Name = "DNB Bank Polska" },
        new() { Name = "Ikano Bank" },
        new() { Name = "InBank" },
        new() { Name = "ING Bank Śląski" },
        new() { Name = "mBank" },
        new() { Name = "Nest Bank S.A." },
        new() { Name = "PKO Bank Polski" },
        new() { Name = "Plus Bank" },
        new() { Name = "Santander Bank Polska" },
        new() { Name = "Santander Consumer Bank" },
        new() { Name = "SGB Bank" },
        new() { Name = "Toyota Bank Polska" },
        new() { Name = "Velo Bank" },
    ];

    public static async Task SeedRequiredDataAsync(AppDbContext context)
    {
        foreach (Bank bank in Banks)
        {
            if (!context.Banks.Any(b => b.Name == bank.Name))
                await context.Banks.AddAsync(bank);
        }

        await context.SaveChangesAsync();
    }

    public static async Task SeedDebugDataAsync(AppDbContext context)
    {
        await SeedTestClient(context);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTestClient(AppDbContext context)
    {
        context.Clients.RemoveRange(context.Clients);

        Client client = new()
        {
            JoinDateTime = DateTime.Now,
            Bank = context.Banks.First(),
        };

        await context.Clients.AddAsync(client);
    }
}