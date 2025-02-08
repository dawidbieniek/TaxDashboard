namespace TaxDashboard.Services.Notifications;
public record NotificationInfo(NotificationSeverity Severity = NotificationSeverity.None, string ? Message = null);

public enum NotificationSeverity
{
    None,
    Warning,
    Danger
}

public static class NotificationSeverityExtensions
{
    public static string TextClass(this NotificationSeverity severity) => severity switch
    {
        NotificationSeverity.Warning => "text-warning",
        NotificationSeverity.Danger => "text-danger",
        _ => "text-secondary"
    };
}
