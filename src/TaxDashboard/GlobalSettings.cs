using System.Globalization;

namespace TaxDashboard;

public static class GlobalSettings
{
    public const NumberStyles CurrencyNumberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowCurrencySymbol | NumberStyles.Currency | NumberStyles.AllowThousands;
    public static readonly CultureInfo CurrencyCulture = CultureInfo.CreateSpecificCulture("pl-PL");

    public static class PreferencesStorage
    {
        public const string LastClientIdKey = "ClientId";
        public const string LastDateContextKey = "DateContext";
        public const string EmailTemplateAmountFiscalKey = "AmountFiscalTemplate";
        public const string EmailTemplateAmountVATKey = "AmountVATTemplate";
        public const string EmailTemplateTimeStartKey = "TimeStartTemplate";
        public const string EmailTemplateTimePreferentialFullKey = "TimePreferentialFullTemplate";
        public const string EmailTemplateTimePreferentialPlusKey = "TimePreferentialPlusTemplate";
        public const string EmailTemplateAmountFiscalSubjectKey = "AmountFiscalTemplateSubject";
        public const string EmailTemplateAmountVATSubjectKey = "AmountVATTemplateSubject";
        public const string EmailTemplateTimeStartSubjectKey = "TimeStartTemplateSubject";
        public const string EmailTemplateTimePreferentialFullSubjectKey = "TimePreferentialFullTemplateSubject";
        public const string EmailTemplateTimePreferentialPlusSubjectKey = "TimePreferentialPlusTemplateSubject";
        public const string EmailTemplateAmountFiscalDangerKey = "AmountFiscalDangerTemplate";
        public const string EmailTemplateAmountVATDangerKey = "AmountVATDangerTemplate";
        public const string EmailTemplateTimeStartDangerKey = "TimeStartDangerTemplate";
        public const string EmailTemplateTimePreferentialFullDangerKey = "TimePreferentialFullDangerTemplate";
        public const string EmailTemplateTimePreferentialPlusDangerKey = "TimePreferentialPlusDangerTemplate";
        public const string EmailTemplateAmountFiscalDangerSubjectKey = "AmountFiscalDangerTemplateSubject";
        public const string EmailTemplateAmountVATDangerSubjectKey = "AmountVATDangerTemplateSubject";
        public const string EmailTemplateTimeStartDangerSubjectKey = "TimeStartDangerTemplateSubject";
        public const string EmailTemplateTimePreferentialFullDangerSubjectKey = "TimePreferentialFullDangerTemplateSubject";
        public const string EmailTemplateTimePreferentialPlusDangerSubjectKey = "TimePreferentialPlusDangerTemplateSubject";
        public const string LastBackupDateKey = "LastBackup";
        public const string DateStorageFormat = "yyyy-MM-dd";
        public const string EmailNameKey = "EmailName";
    }
    public static class SecureStorage
    {
        public const string EmailAddressKey = "EmailAddress";
        public const string EmailPasswordKey = "EmailPassword";
    }

    public static class Emails
    {
        public const string GmailSmtpAddress = "smtp.gmail.com";
        public const int GmailSmtpPort = 465;
        public const int GmailSmtpTSLPort = 587;
    }
}