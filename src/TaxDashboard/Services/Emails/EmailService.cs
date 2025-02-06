using System.ComponentModel;

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

    public Task<LoginResult> LoginUsingOAuthAsync() => _oAuthAuthenticator.LoginAsync();

    public async Task ClearAllLoginDataAsync()
    {
        PasswordAuthenticator.ClearAuthentication();
        await _oAuthAuthenticator.ClearAuthenticationAsync();
    }
}

internal record EmailLoginInfo(bool LoggedIn, string? EmailAddress, EmailLoginOption LoginOption);