using System.ComponentModel;

namespace TaxDashboard.Models.Enums;

public enum TaxType
{
    [Description("Skala podatkowa")]
    Scale,
    [Description("Ryczałt")]
    LumpSum,
    [Description("Podatek liniowy")]
    Linear,
}