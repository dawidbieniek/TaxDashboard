namespace TaxDashboard.Util;
public static class DateTimeExtensions
{
    public static int DaysUntilEndOfYear(this DateTime date) => (new DateTime(date.Year + 1, 1, 1) - date).Days;
}
