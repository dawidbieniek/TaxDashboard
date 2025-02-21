using System.ComponentModel;

namespace TaxDashboard.Data.Enums;

public enum Gender
{
    [Description("Mężczyzna")]
    Male,
    [Description("Kobieta")]
    Female
}

public static class GenderExtensions
{
    public static string GetLocativePronoun(this Gender gender) => gender switch
    {
        Gender.Male => "Pan",
        Gender.Female => "Pani",
        _ => "Pan",
    };

    public static string GetGreeting(this Gender gender) => gender switch
    {
        Gender.Male => "Szanowny Panie",
        Gender.Female => "Szanowna Pani",
        _ => "Szanowny Panie",
    };
}