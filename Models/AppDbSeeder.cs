﻿using TaxDashboard.Models.Entities;

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
        await SeedIncomes(context);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTestClient(AppDbContext context)
    {
        context.Clients.RemoveRange(context.Clients);
        context.VATUEs.RemoveRange(context.VATUEs);
        context.JPKV7s.RemoveRange(context.JPKV7s);

        JPKV7 jpk = new()
        {
            ChangedDateTime = new(2024, 1, 1, 10, 20, 10),
            ContextDate = new(2024, 1, 1),
        };
        VATUE vatue = new()
        {
            ChangedDateTime = new(2024, 1, 1, 10, 20, 10),
            ContextDate = new(2024, 2, 1),
        };

        Client client = new()
        {
            Name = "Test",
            Surname = "Client",
            JoinDateTime = DateTime.Now,
            Bank = context.Banks.First(),
        };
        client.JPKV7HandledDates.Add(jpk);
        client.VATUEHandledDates.Add(vatue);

        Client client2 = new()
        {
            Name = "Test2",
            Surname = "Client2",
            JoinDateTime = DateTime.Now,
            Bank = context.Banks.First(b => b.Name == Banks[5].Name),
        };

        await context.Clients.AddAsync(client);
        await context.Clients.AddAsync(client2);
    }

    private static async Task SeedIncomes(AppDbContext context)
    {
        context.Incomes.RemoveRange(context.Incomes);

        Client client = context.Clients.Local.FirstOrDefault() ?? context.Clients.First();

        List<Income> incomes = [
            new() { Client = client, Amount = 1000, Date = new(2024, 1, 1) },
            new() { Client = client, Amount = 500, Date = new(2024, 2, 1) },
            new() { Client = client, Amount = 1500, Date = new(2024, 3, 1) },
            new() { Client = client, Amount = 2000, Date = new(2024, 4, 1) },
            new() { Client = client, Amount = 500, Date = new(2024, 1, 1) },
            new() {Client = client, Amount = 100, Date = new(2024, 6, 1) },
        ];

        await context.AddRangeAsync(incomes);
    }
}