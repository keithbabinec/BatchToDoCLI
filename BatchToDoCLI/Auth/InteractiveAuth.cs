using Microsoft.Identity.Client;

namespace BatchToDoCLI.Auth
{
    public class InteractiveAuth
    {
        public static async Task<AuthenticationResult> AcquireToken(AuthSettings settings)
        {
            IPublicClientApplication pca = PublicClientApplicationBuilder
                .Create(settings.AppId)
                .WithAuthority(settings.Authority)
                //.WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
                .WithRedirectUri("http://localhost")
                .Build();

            var accounts = await pca.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();

            try
            {
                return await pca.AcquireTokenSilent(settings.Scopes, firstAccount).ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                Console.WriteLine("Interactive logon required.");
                return await pca.AcquireTokenInteractive(settings.Scopes).ExecuteAsync();
            }
        }
    }
}
