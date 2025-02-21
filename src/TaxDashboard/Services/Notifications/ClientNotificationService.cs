using TaxDashboard.Data.Entities;
using TaxDashboard.Data.Enums;
using TaxDashboard.Services.Emails;
using TaxDashboard.Services.Notifications;
using TaxDashboard.Util;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace TaxDashboard.Services;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class ClientNotificationService(ClientsService clientsService)
{
    private const int ReductionWarningDays = 7;
    private const decimal WarningPoint = 0.9m;

    public static NotificationInfo GetClientReductionNotification(Client client)
    {
        int? reductionEndDays = GetDaysUntilEndOfReduction(client);

        if (!reductionEndDays.HasValue)
            return new();

        if (reductionEndDays <= 0)
            return new(NotificationSeverity.Danger, GetReductionDangerMessage(client));
        else if (reductionEndDays <= ReductionWarningDays)
            return new(NotificationSeverity.Warning, GetReductionWarningMessage(client, reductionEndDays.Value));

        return new();
    }

    private readonly ClientsService _clientsService = clientsService;

    public NotificationInfo GetClientLimitNotification(Client client, DateOnly contextDate, NotificationLimitType limitType)
    {
        if ((limitType == NotificationLimitType.Zus && client.ReductionType != ReductionType.ZUSPlus)
            || (limitType == NotificationLimitType.Fiscal && !client.UseCashRegister))
            return new();

        decimal incomeSum = limitType switch
        {
            NotificationLimitType.Vat => ClientsService.GetVatIncomeSum(client, contextDate),
            NotificationLimitType.Fiscal => ClientsService.GetFiscalIncomeSum(client, contextDate),
            NotificationLimitType.Zus => ClientsService.GetZusIncomeSum(client, contextDate),
            _ => 0
        };

        decimal limit = limitType switch
        {
            NotificationLimitType.Vat => ClientsService.GetVatLimit(client, contextDate),
            NotificationLimitType.Fiscal => ClientsService.FiscalLimit,
            NotificationLimitType.Zus => ClientsService.ZusPlusLimit,
            _ => 0
        };

        if (incomeSum >= limit)
            return new(NotificationSeverity.Danger, GetLimitDangerMessage(client, limitType));
        else if (incomeSum >= limit * WarningPoint)
            return new(NotificationSeverity.Warning, GetLimitWarningMessage(client, limitType));

        return new();
    }

    public IEnumerable<NotificationInfo> GetAllClientLimitNotifications(Client client, DateOnly contextDate)
    {
        List<NotificationInfo> notifications = new();
        foreach (NotificationLimitType limitType in Enum.GetValues<NotificationLimitType>())
        {
            NotificationInfo notification = GetClientLimitNotification(client, contextDate, limitType);
            if (notification.Severity != NotificationSeverity.None)
                notifications.Add(notification);
        }
        return notifications;
    }

    private static string GetLimitWarningMessage(Client client, NotificationLimitType limitType) =>
        limitType switch
        {
            NotificationLimitType.Vat => $"Klient <i>{client.FullName}</i> niedługo przekroczy limit VAT.<br/>Kliknij <a href='/email/new?clientId={client.Id}&templateType={(int)EmailTemplateType.AmountVAT}'>tutaj</a>, aby go poinformować",
            NotificationLimitType.Fiscal => $"Klient <i>{client.FullName}</i> niedługo przekroczy limit kasowy.<br/>Kliknij <a href='/email/new?clientId={client.Id}&templateType={(int)EmailTemplateType.AmountFiscal}'>tutaj</a>, aby go poinformować",
            NotificationLimitType.Zus => $"Klient <i>{client.FullName}</i> niedługo przekroczy limit ZUS.<br/>Kliknij <a href='/email/new?clientId={client.Id}'>tutaj</a>, aby go poinformować",
            _ => string.Empty
        };

    private static string GetLimitDangerMessage(Client client, NotificationLimitType limitType) =>
        limitType switch
        {
            NotificationLimitType.Vat => $"Klient <i>{client.FullName}</i> przekroczył limit VAT.<br/>Kliknij <a href='/email/new?clientId={client.Id}&templateType={(int)EmailTemplateType.AmountVATDanger}'>tutaj</a>, aby go poinformować",
            NotificationLimitType.Fiscal => $"Klient <i>{client.FullName}</i> przekroczył limit kasowy.<br/>Kliknij <a href='/email/new?clientId={client.Id}&templateType={(int)EmailTemplateType.AmountFiscalDanger}'>tutaj</a>, aby go poinformować",
            NotificationLimitType.Zus => $"Klient <i>{client.FullName}</i> przekroczył limit ZUS.<br/>Kliknij <a href='/email/new?clientId={client.Id}'>tutaj</a>, aby go poinformować",
            _ => string.Empty
        };

    private static int? GetDaysUntilEndOfReduction(Client client)
    {
        DateOnly? reductionEndDate = client.LastDayOfReduction;

        return reductionEndDate.HasValue
            ? (reductionEndDate.Value.ToDateTime(new()) - DateTime.Today).Days
            : null;
    }

    private static string GetReductionWarningMessage(Client client, int daysToEnd)
    {
        bool eligibleForZusPlus = ClientsService.GetZusIncomeSum(client, DateOnly.FromDateTime(DateTime.Today.AddYears(-1))) > ClientsService.ZusPlusLimit;
        string zusPlusMessage = eligibleForZusPlus
            ? "Może jednak przejść na Mały ZUS Plus"
            : string.Empty;

        return $"Za {daysToEnd} {(daysToEnd == 1 ? "dzień" : "dni")} klient <i>{client.FullName}</i> przekroczy okres ulgi <b>{client.ReductionType.GetDescriptor()}</b>." +
            zusPlusMessage +
            $"<br/>Kliknij <a href='/email/new?clientId={client.Id}&templateType={GetReductionType(client, eligibleForZusPlus)}'>tutaj</a>, aby go poinformować";
    }

    private static string GetReductionDangerMessage(Client client)
    {
        bool eligibleForZusPlus = ClientsService.GetZusIncomeSum(client, DateOnly.FromDateTime(DateTime.Today.AddYears(-1))) > ClientsService.ZusPlusLimit;
        string zusPlusMessage = eligibleForZusPlus
            ? "Może jednak przejść na Mały ZUS Plus"
            : string.Empty;

        return $"Klient <i>{client.FullName}</i> przekroczył okres ulgi <b>{client.ReductionType.GetDescriptor()}</b>." +
            zusPlusMessage +
            $"<br/>Kliknij <a href='/email/new?clientId={client.Id}&templateType={GetReductionTypeOverLimit(client)}'>tutaj</a>, aby go poinformować";
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
    private static int? GetReductionType(Client client, bool eligibleForZusPlus = false)
    {
        if (client.ReductionType == ReductionType.Start)
            return (int)EmailTemplateType.TimeStart;
        if (client.ReductionType == ReductionType.PrefZUS)
        {
            return eligibleForZusPlus
                ? (int)EmailTemplateType.TimePreferentialFull
                : (int)EmailTemplateType.TimePreferentialPlus;
        }

        return null;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
    private static int? GetReductionTypeOverLimit(Client client, bool eligibleForZusPlus = false)
    {
        if (client.ReductionType == ReductionType.Start)
            return (int)EmailTemplateType.TimeStartDanger;
        if (client.ReductionType == ReductionType.PrefZUS)
        {
            return eligibleForZusPlus
                ? (int)EmailTemplateType.TimePreferentialFullDanger
                : (int)EmailTemplateType.TimePreferentialPlusDanger;
        }

        return null;
    }
}