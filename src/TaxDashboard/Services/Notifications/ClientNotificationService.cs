using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaxDashboard.Data.Entities;
using TaxDashboard.Services.Notifications;
using TaxDashboard.Util;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace TaxDashboard.Services;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class ClientNotificationService
{
    private const int ReductionWarningDays = 7;
    private const int StartReductionValidMonths = 6;
    private const int PreferentialReductionValidYears = 2;
    private const int PlusReductionValidYears = 3;

    public static NotificationInfo GetClientReductionNotification(Client client)
    {
        int reductionEndDays = GetDaysUntilEndOfReduction(client);

        if (reductionEndDays <= 0)
            return new(NotificationSeverity.Danger, GetReductionDangerMessage(client));
        else if(reductionEndDays <= ReductionWarningDays) 
            return new(NotificationSeverity.Warning, GetReductionWarningMessage(client, reductionEndDays));

        return new();
    }

    private static int GetDaysUntilEndOfReduction(Client client)
    {
        DateOnly reductionEndDate = client.ReductionType switch
        {
            Data.Enums.ReductionType.Start => reductionEndDate = client.ReductionChangeDate.AddMonths(StartReductionValidMonths),
            Data.Enums.ReductionType.PrefZUS => reductionEndDate = client.ReductionChangeDate.AddYears(PreferentialReductionValidYears),
            Data.Enums.ReductionType.ZUSPlus => reductionEndDate = client.ReductionChangeDate.AddMonths(PlusReductionValidYears),
            _ => reductionEndDate = DateOnly.MaxValue
        };

        return (reductionEndDate.ToDateTime(new()) - DateTime.Today).Days;
    }

    private static string GetReductionWarningMessage(Client client, int daysToEnd) => 
        $"Za {daysToEnd} {(daysToEnd == 1? "dzień" : "dni")} klient {client.FullName} przekroczy okres ulgi <b>{client.ReductionType.GetDescriptor()}</b>." +
        $"<br/>Kliknij <a href='/email/new?clientId={client.Id}'>tutaj</a>, aby go poinformować";

    private static string GetReductionDangerMessage(Client client) =>
        $"Klient {client.FullName} przekroczył okres ulgi <b>{client.ReductionType.GetDescriptor()}</b>." +
        $"<br/>Kliknij <a href='/email/new?clientId={client.Id}'>tutaj</a>, aby go poinformować";
}