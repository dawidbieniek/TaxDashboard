using Microsoft.EntityFrameworkCore;

using TaxDashboard.Models;

namespace TaxDashboard;

internal static class DependencyInjection
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        //File.Delete(AppDbContextExtensions.DatabaseFilePath);

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Filename={AppDbContextExtensions.DatabaseFilePath}"));

        return services;
    }
}