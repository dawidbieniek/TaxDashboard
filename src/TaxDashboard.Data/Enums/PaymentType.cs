using System.ComponentModel;

namespace TaxDashboard.Data.Enums;

public enum PaymentType
{
    [Description("Miesięcznie")]
    Monthly,
    [Description("Kwartalnie")]
    Quarterly,
}