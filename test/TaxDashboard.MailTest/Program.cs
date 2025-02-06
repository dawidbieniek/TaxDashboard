using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;

using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Configuration;

using MimeKit;

namespace TaxDashboard.MailTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Starting");

            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            Console.WriteLine("Loaded secrets");

            MimeMessage message = new();
            message.From.Add(new MailboxAddress("TestMail", config["SenderAddress"]));
            message.To.Add(new MailboxAddress("TestReciever", config["RecieverAddress"]));

            message.Subject = "Test message - HTML?";
            message.Body = new TextPart("html")
            {
                Text = """
                This is a test message content
                Its goal is to check out MailKit package
                and its capabilities in sending emails.

                <b>This text will be bolded if send as HTML</b>
                """
            };

            Console.WriteLine("## Message contents ##");
            Console.Write("Subject: ");
            Console.WriteLine(message.Subject);
            Console.WriteLine();
            Console.WriteLine(message.Body);

            Console.WriteLine("Authorization method:");
            Console.WriteLine("[1] Password");
            Console.WriteLine("[2] OAuth");

            char key = Console.ReadKey().KeyChar;
            Console.WriteLine();
            switch (key)
            {
                case '1':
                    SendUsingPasswordAuth(config, message);
                    break;

                case '2':
                    await SendUsingOAuthAsync(config, message);
                    break;

                default:
                    Console.WriteLine($"Unrecognised option: {key}");
                    break;
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void SendUsingPasswordAuth(IConfiguration config, MimeMessage message)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(config["SenderAddress"], config["SenderPassword"]);
                client.Send(message);

                Console.WriteLine("Message sent using password authorization");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                client.Disconnect(true);
            }
        }

        private static async Task SendUsingOAuthAsync(IConfiguration config, MimeMessage message)
        {
            const string GMailAccount = "dawid.b1101@gmail.com";
            var clientSecrets = new ClientSecrets
            {
                ClientId = config["OAuth:ClientId"],
                ClientSecret = config["OAuth:ClientSecret"]
            };

            var tokenDataStore = new FileDataStore("TaxDasboard.Tests/CredentialCacheFolder", false);
            Console.WriteLine($"Cache folder: {tokenDataStore.FolderPath}");

            var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = ["https://mail.google.com/"],
                DataStore = tokenDataStore,
                LoginHint = GMailAccount
            });

            var codeReceiver = new LocalServerCodeReceiver();
            var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);

            using var client = new SmtpClient();
            try
            {
                // Get credentials
                var credential = await authCode.AuthorizeAsync(GMailAccount, CancellationToken.None);

                // Check if token needs refreshing
                if (credential.Token.IsStale) await credential.RefreshTokenAsync(CancellationToken.None);

                // Create OAuth2 mechanism
                var oauth2 = new SaslMechanismOAuthBearer(credential.UserId, credential.Token.AccessToken);

                // Send email using MailKit
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(oauth2);

                await client.SendAsync(message);

                Console.WriteLine("Message sent using OAuth");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}