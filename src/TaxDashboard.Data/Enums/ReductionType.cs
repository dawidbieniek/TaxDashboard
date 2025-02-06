using System.ComponentModel;

namespace TaxDashboard.Data.Enums;

public enum ReductionType
{
    [Description("Ulga na start")]
    Start,
    [Description("ZUS preferencyjny")]
    PrefZUS,
    [Description("Mały ZUS Plus")]
    ZUSPlus,
    [Description("Pełny ZUS")]
    FullZUS,
}