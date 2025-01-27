using TaxDashboard.Initialization;
using TaxDashboard.Models;
using TaxDashboard.Services;

namespace TaxDashboard;

internal static class DependencyInjection
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        DatabaseInitializer.RegisterDataServcies(services);
        services.AddTransient<InitializationPage>();
        services.AddTransient<TestService>();

        return services;
    }
}