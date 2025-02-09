using MailKit.Net.Smtp;

namespace TaxDashboard.Services.Emails;

internal class PasswordAuthenticator
{
    public static async Task<string?> GetAuthenticatedUserEmailAddressAsync()
    {
        string? password = await SecureStorage.GetAsync(GlobalSettings.SecureStorage.EmailPasswordKey);
        if (string.IsNullOrEmpty(password))
            return null;

        return await SecureStorage.GetAsync(GlobalSettings.SecureStorage.EmailAddressKey);
    }

    public static void ClearAuthentication()
    {
        SecureStorage.Remove(GlobalSettings.SecureStorage.EmailAddressKey);
        SecureStorage.Remove(GlobalSettings.SecureStorage.EmailPasswordKey);
    }

    public static async Task<LoginResult> LoginAsync(string emailAddress, string password)
    {
        if (!await AuthenticateUsingPasswordAsync(emailAddress, password))
            return new(false, "Niepoprawne dane logowania");

        await SecureStorage.SetAsync(GlobalSettings.SecureStorage.EmailAddressKey, emailAddress);
        await SecureStorage.SetAsync(GlobalSettings.SecureStorage.EmailPasswordKey, password);

        return new();
    }

    public static async Task<(string email, string password)?> GetStoredAuthenticationData()
    {
        string? email = await SecureStorage.GetAsync(GlobalSettings.SecureStorage.EmailAddressKey);
        string? password = await SecureStorage.GetAsync(GlobalSettings.SecureStorage.EmailPasswordKey);

        return string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email) ? null : (email, password);
    }

    private static async Task<bool> AuthenticateUsingPasswordAsync(string emailAddress, string password)
    {
        using SmtpClient client = new();

        try
        {
            await client.ConnectAsync(GlobalSettings.Emails.GmailSmtpAddress, GlobalSettings.Emails.GmailSmtpPort, true);
            await client.AuthenticateAsync(emailAddress, password);
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