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

    public static void SeedData(AppDbContext context)
    {
        if (!context.Banks.Any())
            context.Banks.AddRange(Banks);

        SeedTestClient(context);

        context.SaveChanges();
    }

    private static void SeedTestClient(AppDbContext context)
    {
#if !DEBUG
        return;
#endif

        //if (context.Clients.Any())
        //    return;
        context.Clients.RemoveRange(context.Clients);

        Client client = new()
        {
            JoinDateTime = DateTime.Now,
            Bank = Banks[0],
        };

        context.Clients.Add(client);
    }
}