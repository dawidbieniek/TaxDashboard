namespace TaxDashboard.Util;

public static class DataDisplayHelper
{
    private static readonly string[] MonthLabels = ["Sty", "Lut", "Mar", "Kwi", "Maj", "Cze", "Lip", "Sie", "Wrz", "Paź", "Lis", "Gru"];
    private static readonly string[] MonthFullLabels = ["Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień"];

    /// <summary>
    /// Returns month label for given month.
    /// </summary>
    /// <remarks> Months are counted from 1 </remarks>
    public static string GetMonthLabel(int month)
    {
        if (month < 1 || month > 12)
            return "Err";
        return MonthLabels[month - 1];
    }

    public static string GetMonthFullLabel(int month)
    {
        if (month < 1 || month > 12)
            return "Error";
        return MonthFullLabels[month - 1];
    }
}