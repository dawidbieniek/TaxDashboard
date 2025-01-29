using System.Globalization;

namespace TaxDashboard;

public static class GlobalSettings
{
    public const NumberStyles CurrencyNumberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowCurrencySymbol;
    public static readonly CultureInfo CurrencyCulture = CultureInfo.CreateSpecificCulture("pl-PL");
}