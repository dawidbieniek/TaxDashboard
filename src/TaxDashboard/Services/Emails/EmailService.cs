using System.ComponentModel;

using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;

using TaxDashboard.Services.Emails;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace TaxDashboard.Services;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal enum EmailLoginOption
{
    None,
    [Description("Hasło")]
    Password,
    [Description("OAuth")]
    OAuth
}

internal partial class EmailService(OAuthAuthenticator oAuthAuthenticator)
{
    private readonly OAuthAuthenticator _oAuthAuthenticator = oAuthAuthenticator;

    public bool IsOAuthInvalid => _oAuthAuthenticator.IsInvalid;

    public static Task<LoginResult> LoginUsingPasswordAsync(string emailAddress, string password) => PasswordAuthenticator.LoginAsync(emailAddress, password);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
    public async Task<EmailLoginInfo> GetCurrentLoginInfoAsync()
    {
        string? oAuthEmail = await _oAuthAuthenticator.GetAuthenticatedUserEmailAddressAsync();
        if (!string.IsNullOrEmpty(oAuthEmail))
            return new(true, oAuthEmail, EmailLoginOption.OAuth);

        string? email = await PasswordAuthenticator.GetAuthenticatedUserEmailAddressAsync();
        if (!string.IsNullOrEmpty(email))
            return new(true, email, EmailLoginOption.Password);

        return new(false, null, EmailLoginOption.None);
    }

    public Task<LoginResult?> LoginUsingOAuthAsync() => _oAuthAuthenticator.LoginAsync();

    public async Task ClearAllLoginDataAsync()
    {
        PasswordAuthenticator.ClearAuthentication();
        await _oAuthAuthenticator.ClearAuthenticationAsync();
    }

    public async Task<bool> SendEmailAsync(string recieverAddress, string recieverName, string senderName, string subject, string content)
    {
        MimeMessage message = new();
        message.To.Add(new MailboxAddress(recieverName, recieverAddress));
        message.Subject = subject;
        message.Body = new TextPart("html")
        {
            Text = content
        };

        SaslMechanism? oAuthAuthentication = await _oAuthAuthenticator.GetStoredAuthenticationData();
        if (oAuthAuthentication is not null)
        {
            message.From.Add(new MailboxAddress(senderName, oAuthAuthentication.Credentials.UserName));
            return await SendEmailUsingOAuthAsync(oAuthAuthentication, message);
        }

        var passwordAuthentication = await PasswordAuthenticator.GetStoredAuthenticationData();
        if (passwordAuthentication is not null)
        {
            message.From.Add(new MailboxAddress(senderName, passwordAuthentication.Value.email));
            return await SendEmailUsingPasswordAsync(passwordAuthentication.Value.email, passwordAuthentication.Value.password, message);
        }

        return false;
    }

    private static async Task<bool> SendEmailUsingOAuthAsync(SaslMechanism auth, MimeMessage message)
    {
        using SmtpClient client = new();
        try
        {
            await client.ConnectAsync(GlobalSettings.Emails.GmailSmtpAddress, GlobalSettings.Emails.GmailSmtpTSLPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(auth);
            await client.SendAsync(message);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }

    private static async Task<bool> SendEmailUsingPasswordAsync(string email, string password, MimeMessage message)
    {
        using SmtpClient client = new();
        try
        {
            await client.ConnectAsync(GlobalSettings.Emails.GmailSmtpAddress, GlobalSettings.Emails.GmailSmtpPort, true);
            await client.AuthenticateAsync(email, password);
            await client.SendAsync(message);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}

internal record EmailLoginInfo(bool LoggedIn, string? EmailAddress, EmailLoginOption LoginOption);