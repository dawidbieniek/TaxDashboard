using System.Globalization;

namespace TaxDashboard;

public static class GlobalSettings
{
    public const NumberStyles CurrencyNumberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowCurrencySymbol | NumberStyles.Currency | NumberStyles.AllowThousands;
    public static readonly CultureInfo CurrencyCulture = CultureInfo.CreateSpecificCulture("pl-PL");
}