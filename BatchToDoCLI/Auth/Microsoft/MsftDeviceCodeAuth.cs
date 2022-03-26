using Microsoft.Identity.Client;

namespace BatchToDoCLI.Auth.Microsoft
{
    public class MsftDeviceCodeAuth
    {
        public static async Task<AuthenticationResult> GetATokenForGraph(MsftAuthSettings settings)
        {
            IPublicClientApplication pca = PublicClientApplicationBuilder
                .Create(settings.AppId)
                .WithAuthority(settings.Authority)
                .WithDefaultRedirectUri()
                .Build();

            var accounts = await pca.GetAccountsAsync();

            try
            {
                // All AcquireToken* methods store the tokens in the cache, so check the cache first
                return await pca.AcquireTokenSilent(settings.Scopes, accounts.FirstOrDefault()).ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // No token found in the cache or AAD insists that a form interactive auth is required (e.g. the tenant admin turned on MFA)
                // If you want to provide a more complex user experience, check out ex.Classification
                return await AcquireByDeviceCodeAsync(pca, settings);
            }
        }

        private static async Task<AuthenticationResult> AcquireByDeviceCodeAsync(IPublicClientApplication pca, MsftAuthSettings settings)
        {
            try
            {
                var result = await pca.AcquireTokenWithDeviceCode(settings.Scopes,
                    deviceCodeResult =>
                    {
                        // print the 'device code' login prompt / url.
                        Console.WriteLine(deviceCodeResult.Message);
                        return Task.FromResult(0);
                    }).ExecuteAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
