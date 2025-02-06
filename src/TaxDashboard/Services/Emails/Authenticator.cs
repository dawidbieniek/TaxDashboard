namespace TaxDashboard.Services.Emails;

internal abstract class Authenticator
{
    protected const string GmailSmtpAddress = "smtp.gmail.com";
    protected const int GmailSmtpPort = 465;
    protected const int GmailSmtpTSLPort = 587;
}