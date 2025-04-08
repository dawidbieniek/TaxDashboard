using System.ComponentModel;

namespace TaxDashboard.Services.Calculator;

public enum ContributionVariant
{
    [Description("Preferencyjny")]
    Preferential,
    [Description("Preferencyjny z chorobową")]
    PreferentialWithIllness,
    [Description("Pełny ZUS")]
    FullZUS,
    [Description("Pełny ZUS z chorobową")]
    FullZUSWithIllness
}