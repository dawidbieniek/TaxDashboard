using Microsoft.EntityFrameworkCore;

using TaxDashboard.Models.Entities;

namespace TaxDashboard.Models;

public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; } = default!;
    public DbSet<JPKV7> JPKV7s { get; set; } = default!;
    public DbSet<Bank> Banks { get; set; } = default!;
}

public static class AppDbContextExtensions
{
    public static readonly string DatabaseFilePath = Path.Combine(FileSystem.AppDataDirectory, "data.db3");

    public static void CreateDbIfNotExists(this IServiceProvider provider)
    {
        using IServiceScope scope = provider.CreateScope();
        IServiceProvider services = scope.ServiceProvider;

        AppDbContext dbContext = services.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureCreated();
        AppDbSeeder.SeedData(dbContext);
    }
}