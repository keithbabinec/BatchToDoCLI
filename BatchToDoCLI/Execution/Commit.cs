using BatchToDoCLI.Auth;
using BatchToDoCLI.Models;
using Microsoft.Extensions.Configuration;

namespace BatchToDoCLI.Execution
{
    public class Commit
    {
        public async Task<ExitCodes> RunAsync(Settings settingsHelper, IConfigurationRoot settings, CommandArguments cmdArgs, TaskBatch batchTransformed)
        {
            // authenticate to Microsoft Identity Platform and obtain a Graph API token.
            // token is good for an hour. cache it in user env vars if the option was passed.

            var authSettings = settingsHelper.GetAuthSettings(settings);

            Console.WriteLine("Auth configuration loaded.");

            string accessToken = String.Empty;

            if (cmdArgs.CacheAuthTokens)
            {
                if (!TokenCache.GetTokenFromCache(out accessToken))
                {
                    Console.WriteLine("No recent cached token found. Will need to login.");
                }
                else
                {
                    Console.WriteLine("Grabbed a token from the cache.");
                }
            }

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                var res = await DeviceCodeAuth.GetATokenForGraph(authSettings);

                if (res == null)
                {
                    // no need to print an error message here.
                    // an exception is thrown and caught within the token call, and is printed to the STDERR
                    // already by the time this is reached.
                    return ExitCodes.FailedToAuthenticate;
                }
                else
                {
                    Console.WriteLine(Environment.NewLine + "Logged in as " + res.Account.Username);
                    accessToken = res.AccessToken;

                    if (cmdArgs.CacheAuthTokens)
                    {
                        TokenCache.SaveTokenToCache(res.AccessToken, DateTime.Now.AddMinutes(60));
                        Console.WriteLine("Token saved to the cache (valid for 1 hour).");
                    }
                }
            }

            // call the helper method that actually invokes the graph API to create the batch.

            var graphUri = settings[Constants.GraphApiBaseUri];
            var tasker = new TaskCreator(graphUri);
            return await tasker.CreateBatchAsync(accessToken, batchTransformed);
        }
    }
}
