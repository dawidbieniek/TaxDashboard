using TaxDashboard.Initialization;
using TaxDashboard.Services;
using TaxDashboard.Services.Emails;

namespace TaxDashboard;

internal static class DependencyInjection
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddTransient<InitializationPage>();
        services.AddTransient<ClientsService>();
        services.AddTransient<BanksService>();
        services.AddTransient<BackupService>();
        services.AddTransient<OAuthAuthenticator>();
        services.AddTransient<EmailService>();
        return services;
    }
}