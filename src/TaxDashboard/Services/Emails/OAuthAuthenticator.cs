using System.Text.RegularExpressions;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;

using LifeManagers.Data;

using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TaxDashboard.Services.Emails;

internal partial class OAuthAuthenticator
{
    private const string UserId = "appUser";
    private readonly AuthorizationCodeInstalledApp? _authorizationCodeApp;
    private readonly FileDataStore _tokenStore;
    private readonly IAuthorizationCodeFlow? _oauthCodeFlow;

    public bool IsInvalid { get; private set; } = false;

    public OAuthAuthenticator(IOptions<DataServicesOptions> dataOptions, IConfiguration config)
    {
        _tokenStore = new FileDataStore(Path.Combine(dataOptions.Value.DataDirectoryPath, "OAuthTokens"), true);

        string? clientAppOAuthId = config["OAuth:ClientAppId"];
        string? clientAppOAuthClientSecret = config["OAuth:ClientAppSecret"];
        if (string.IsNullOrEmpty(clientAppOAuthId) || string.IsNullOrEmpty(clientAppOAuthClientSecret))
        {
            IsInvalid = true;
            return;
        }

        _oauthCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer()
        {
            ClientSecrets = new()
            {
                ClientId = clientAppOAuthId,
                ClientSecret = clientAppOAuthClientSecret
            },
            Scopes = ["https://mail.google.com/", "https://www.googleapis.com/auth/userinfo.email"],
            DataStore = _tokenStore
        });

        ICodeReceiver oauthCodeReciever = new LocalServerCodeReceiver(
            @"<html><head><meta charset=""utf-8"" /><title>Logowanie OAuth2.0</title></head><body><h3>Logowanie OAuth zakończone</h3>Można już zamknąć to okno</body></html>",
            LocalServerCodeReceiver.CallbackUriChooserStrategy.ForceLocalhost);

        _authorizationCodeApp = new(_oauthCodeFlow, oauthCodeReciever);
    }

    public async Task<string?> GetAuthenticatedUserEmailAddressAsync()
    {
        if(IsInvalid)
            return null;

        TokenResponse? token = await GetStoredTokenAsync();
        if (token is null)
            return null;

        return await TryRefreshTokenIfNeededAsync(token) 
            ? await GetUserEmailAddressAsync(new(_oauthCodeFlow, UserId, token)) 
            : null;
    }

    public async Task ClearAuthenticationAsync()
    {
        if (IsInvalid)
            return;

        TokenResponse? token = await GetStoredTokenAsync();
        if (token is not null)
            await RevokeTokenAsync(token);

        if (Directory.Exists(_tokenStore.FolderPath))
        {
            foreach (string filePath in Directory.GetFiles(_tokenStore.FolderPath))
                File.Delete(filePath);
        }
    }

    public async Task<LoginResult?> LoginAsync()
    {
        if (IsInvalid)
            return null;

        return await AuthenticateUsingOAuthAsync() ? new() : new(false, "Logowanie nie udało się");
    }

    public async Task<SaslMechanism?> GetStoredAuthenticationData()
    {
        if (IsInvalid)
            return null;

        TokenResponse? token = await GetStoredTokenAsync();
        if (token is null)
            return null;

        if (!await TryRefreshTokenIfNeededAsync(token))
            return null;

        string? email = await GetUserEmailAddressAsync(new(_oauthCodeFlow, UserId, token));
        if (string.IsNullOrEmpty(email))
            return null;

        return new SaslMechanismOAuthBearer(email, token.AccessToken);
    }

    [GeneratedRegex("\"email\"\\s*:\\s*\"(.*?)\"")]
    private static partial Regex EmailAddressFromJsonRegex();

    private static async Task<string?> GetUserEmailAddressAsync(UserCredential credential)
    {
        HttpResponseMessage response;
        using (HttpClient http = new())
        {
            response = await http.GetAsync($"https://www.googleapis.com/oauth2/v2/userinfo?access_token={credential.Token.AccessToken}");
        }
        if (!response.IsSuccessStatusCode)
            return null;

        string responseContents = await response.Content.ReadAsStringAsync();

        Match match = EmailAddressFromJsonRegex().Match(responseContents);
        return match.Success ? match.Groups[1].Value : null;
    }

    private async Task<bool> AuthenticateUsingOAuthAsync()
    {
        SaslMechanismOAuthBearer oauth2;
        try
        {
            UserCredential creds = await _authorizationCodeApp!.AuthorizeAsync(UserId, CancellationToken.None);

            if (creds.Token.IsStale)
                await creds.RefreshTokenAsync(CancellationToken.None);

            string? userId = await GetUserEmailAddressAsync(creds);
            if (string.IsNullOrEmpty(userId))
                return false;

            await SecureStorage.SetAsync(GlobalSettings.SecureStorage.EmailAddressKey, userId);
            oauth2 = new(userId, creds.Token.AccessToken);
        }
        catch (Exception)
        {
            return false;
        }

        using SmtpClient client = new();
        try
        {
            await client.ConnectAsync(GlobalSettings.Emails.GmailSmtpAddress, GlobalSettings.Emails.GmailSmtpTSLPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(oauth2);
            return true;
        }
        catch (Exception)
        {
            // When user gives partial permissions, token will be saved. We need to delete this
            // token so that it is possible to ask for full one in future
            await ClearAuthenticationAsync();
            return false;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }

    private async Task<TokenResponse?> GetStoredTokenAsync() => await _tokenStore.GetAsync<TokenResponse>(UserId);

    private async Task RevokeTokenAsync(TokenResponse token)
    {
        if (token.IsStale)
            return;

        UserCredential creds = new(_oauthCodeFlow, UserId, token);
        await creds.RevokeTokenAsync(CancellationToken.None);
    }

    private async Task<bool> TryRefreshTokenIfNeededAsync(TokenResponse token)
    {
        if (!token.IsStale)
            return true;

        UserCredential creds = new(_oauthCodeFlow, UserId, token);
        try
        {
            await creds.RefreshTokenAsync(CancellationToken.None);
            return true;
        }
        catch
        {
            return false;
        }
    }
}