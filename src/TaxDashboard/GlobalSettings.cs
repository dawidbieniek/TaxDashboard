﻿using System.Globalization;

namespace TaxDashboard;

public static class GlobalSettings
{
    public const NumberStyles CurrencyNumberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowCurrencySymbol | NumberStyles.Currency | NumberStyles.AllowThousands;
    public static readonly CultureInfo CurrencyCulture = CultureInfo.CreateSpecificCulture("pl-PL");

    public static class PreferencesStorage
    {
        public const string LastClientIdKey = "ClientId";
        public const string LastDateContextKey = "DateContext";
        public const string EmailTemplateKey = "EmailTemplate";
        public const string LastBackupDateKey = "LastBackup";
        public const string DateStorageFormat = "yyyy-MM-dd";
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