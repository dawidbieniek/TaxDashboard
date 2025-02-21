using TaxDashboard.Initialization;
using TaxDashboard.Services;
using TaxDashboard.Services.Emails;

using Windows.UI.Notifications;

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
        services.AddTransient<EmailTemplatesService>();
        services.AddTransient<ClientNotificationService>();
        services.AddTransient<EmailService>();
        return services;
    }
}