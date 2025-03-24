namespace TaxDashboard.Services.Notifications;
public record NotificationInfo(NotificationSeverity Severity = NotificationSeverity.None, string? Message = null);

public enum NotificationSeverity
{
    None,
    Warning,
    Danger
}

public static class NotificationSeverityExtensions
{
    public static MudBlazor.Color Color(this NotificationSeverity severity) => severity switch
    {
        NotificationSeverity.Warning => MudBlazor.Color.Warning,
        NotificationSeverity.Danger => MudBlazor.Color.Error,
        _ => MudBlazor.Color.Default
    };
}