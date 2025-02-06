namespace TaxDashboard.Util;

internal static class TimeSpanExtensions
{
    public static string ToHumanReadableString(this TimeSpan timeSpan)
    {
        List<string> parts = new List<string>();

        if (timeSpan.Days > 0)
        {
            string dayWord = timeSpan.Days switch
            {
                1 => "dzień",
                _ when timeSpan.Days % 10 == 2 ||
                     timeSpan.Days % 10 == 3 ||
                     timeSpan.Days % 10 == 4 => "dni",
                _ => "dni"
            };
            parts.Add($"{timeSpan.Days} {dayWord}");
        }

        if (timeSpan.Hours > 0)
        {
            string hourWord = timeSpan.Hours switch
            {
                1 => "godzina",
                _ when timeSpan.Hours % 10 == 2 ||
                     timeSpan.Hours % 10 == 3 ||
                     timeSpan.Hours % 10 == 4 => "godziny",
                _ => "godzin"
            };
            parts.Add($"{timeSpan.Hours} {hourWord}");
        }

        return parts.Count switch
        {
            0 => "0 dni",
            1 => parts[0],
            _ => string.Join(", ", parts)
        };
    }
}
