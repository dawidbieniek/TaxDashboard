using TaxDashboard.Data.Entities;
using TaxDashboard.Data.Enums;
using TaxDashboard.Services.Emails;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace TaxDashboard.Services;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal class EmailTemplatesService
{
    public static readonly string[] AvailableTokens = ["panPani", "szanowny", "imie", "nazwisko", "nip", "dataKoniecUlgi", "dniKoniecUlgi"];

    public static (string subject, string content) GetTemplate(EmailTemplateType type) => (Preferences.Get(GetTemplateSubjectKey(type), string.Empty), Preferences.Get(GetTemplateKey(type), string.Empty));

    public static void SetTemplate(EmailTemplateType type, string subject, string content)
    {
        string subjectKey = GetTemplateSubjectKey(type);
        string contentKey = GetTemplateKey(type);
        if (string.IsNullOrEmpty(subjectKey) || string.IsNullOrEmpty(contentKey))
            return;

        Preferences.Set(subjectKey, subject);
        Preferences.Set(contentKey, content);
    }

    private static string GetTemplateKey(EmailTemplateType type) => type switch
    {
        EmailTemplateType.AmountFiscal => GlobalSettings.PreferencesStorage.EmailTemplateAmountFiscalKey,
        EmailTemplateType.AmountVAT => GlobalSettings.PreferencesStorage.EmailTemplateAmountVATKey,
        EmailTemplateType.TimeStart => GlobalSettings.PreferencesStorage.EmailTemplateTimeStartKey,
        EmailTemplateType.TimePreferentialFull => GlobalSettings.PreferencesStorage.EmailTemplateTimePreferentialFullKey,
        EmailTemplateType.TimePreferentialPlus => GlobalSettings.PreferencesStorage.EmailTemplateTimePreferentialPlusKey,
        EmailTemplateType.AmountFiscalDanger => GlobalSettings.PreferencesStorage.EmailTemplateAmountFiscalDangerKey,
        EmailTemplateType.AmountVATDanger => GlobalSettings.PreferencesStorage.EmailTemplateAmountVATDangerKey,
        EmailTemplateType.TimeStartDanger => GlobalSettings.PreferencesStorage.EmailTemplateTimeStartDangerKey,
        EmailTemplateType.TimePreferentialFullDanger => GlobalSettings.PreferencesStorage.EmailTemplateTimePreferentialFullDangerKey,
        EmailTemplateType.TimePreferentialPlusDanger => GlobalSettings.PreferencesStorage.EmailTemplateTimePreferentialPlusDangerKey,
        _ => string.Empty,
    };

    private static string GetTemplateSubjectKey(EmailTemplateType type) => type switch
    {
        EmailTemplateType.AmountFiscal => GlobalSettings.PreferencesStorage.EmailTemplateAmountFiscalSubjectKey,
        EmailTemplateType.AmountVAT => GlobalSettings.PreferencesStorage.EmailTemplateAmountVATSubjectKey,
        EmailTemplateType.TimeStart => GlobalSettings.PreferencesStorage.EmailTemplateTimeStartSubjectKey,
        EmailTemplateType.TimePreferentialFull => GlobalSettings.PreferencesStorage.EmailTemplateTimePreferentialFullSubjectKey,
        EmailTemplateType.TimePreferentialPlus => GlobalSettings.PreferencesStorage.EmailTemplateTimePreferentialPlusSubjectKey,
        EmailTemplateType.AmountFiscalDanger => GlobalSettings.PreferencesStorage.EmailTemplateAmountFiscalDangerSubjectKey,
        EmailTemplateType.AmountVATDanger => GlobalSettings.PreferencesStorage.EmailTemplateAmountVATDangerSubjectKey,
        EmailTemplateType.TimeStartDanger => GlobalSettings.PreferencesStorage.EmailTemplateTimeStartDangerSubjectKey,
        EmailTemplateType.TimePreferentialFullDanger => GlobalSettings.PreferencesStorage.EmailTemplateTimePreferentialFullDangerSubjectKey,
        EmailTemplateType.TimePreferentialPlusDanger => GlobalSettings.PreferencesStorage.EmailTemplateTimePreferentialPlusDangerSubjectKey,
        _ => string.Empty,
    };

    public static string ReplaceTokensInMessage(string template, Client client)
    {
        TokenParser parser = new();

        parser.RegisterToken("panPani", client.Gender.GetLocativePronoun());
        parser.RegisterToken("szanowny", client.Gender.GetGreeting());
        parser.RegisterToken("imie", client.Name);
        parser.RegisterToken("nazwisko", client.Surname);
        parser.RegisterToken("nip", client.NIP);
        parser.RegisterToken("dataKoniecUlgi", client.LastDayOfReduction?.ToString() ?? string.Empty);
        parser.RegisterToken("dniKoniecUlgi", Math.Max(client.LastDayOfReduction.HasValue ? (client.LastDayOfReduction.Value.ToDateTime(new()) - DateTime.Today).Days : 0, 0).ToString());

        return parser.ReplaceTokens(template);
    }
}