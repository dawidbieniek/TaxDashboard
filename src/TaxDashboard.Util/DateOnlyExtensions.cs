namespace TaxDashboard.Util;
public static class DateOnlyExtensions
{
    public static DateOnly FirstOfCurrentMonth(this DateOnly dateOnly) => new(dateOnly.Year, dateOnly.Month, 1);
    public static DateOnly FirstDayOfCurrentYear(this DateOnly dateOnly) => new(dateOnly.Year, 1, 1);
    public static DateOnly LastDayOfCurrentYear(this DateOnly dateOnly) => new(dateOnly.Year, 12, 31);
}
