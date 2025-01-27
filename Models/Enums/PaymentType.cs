using System.ComponentModel;

namespace TaxDashboard.Models.Enums;

public enum PaymentType
{
    [Description("Miesięcznie")]
    Monthly,
    [Description("Kwartalnie")]
    Quarterly,
}