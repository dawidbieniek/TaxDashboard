using Microsoft.EntityFrameworkCore;

namespace TaxDashboard.Models;

public class DatabaseInitializer(IDbContextFactory<AppDbContext> contextFactory)
{
    public static readonly string DatabaseFilePath = Path.Combine(FileSystem.AppDataDirectory, "data.db3");
    private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

    public event EventHandler<string> StepExecuting = delegate { };

    public static IServiceCollection RegisterDataServcies(IServiceCollection services)
    {
        services.AddDbContextFactory<AppDbContext>(options => options.UseSqlite($"Filename={DatabaseFilePath}"));

        services.AddTransient<DatabaseInitializer>();

        return services;
    }

    public async Task InitializeDatabaseAsync()
    {
        using AppDbContext context = await _contextFactory.CreateDbContextAsync();

        StepExecuting?.Invoke(this, "Creating database");
        await CreateDatabaseFileIfNotExistAsync(context);

        StepExecuting?.Invoke(this, "Migrating database");
        //await context.Database.MigrateAsync();

        StepExecuting?.Invoke(this, "Seeding database");
        await AppDbSeeder.SeedRequiredDataAsync(context);

#if DEBUG
        await AppDbSeeder.SeedDebugDataAsync(context);
#endif
    }

    private static async Task CreateDatabaseFileIfNotExistAsync(AppDbContext context)
    {
        if
    (File.Exists(DatabaseFilePath)) return;

        string directoryPath = Path.GetDirectoryName(DatabaseFilePath)!; if
        (Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        File.Create(DatabaseFilePath).Close();

        await context.Database.EnsureCreatedAsync();
    }
}