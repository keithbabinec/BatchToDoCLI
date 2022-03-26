using BatchToDoCLI.Auth;
using BatchToDoCLI.Auth.Microsoft;
using BatchToDoCLI.Logging;
using BatchToDoCLI.Models;
using Microsoft.Extensions.Configuration;

namespace BatchToDoCLI.Execution.Microsoft
{
    public class MsftToDoTaskCreator : ITaskCreator
    {
        private ILogging Logging;
        private Settings SettingsHelper;
        private IConfigurationRoot Settings;
        private CommandArguments CmdArgs;

        public MsftToDoTaskCreator(ILogging logging, Settings settingsHelper, IConfigurationRoot settings, CommandArguments cmdArgs)
        {
            if (logging == null)
            {
                throw new ArgumentNullException(nameof(logging));
            }
            if (settingsHelper == null)
            {
                throw new ArgumentNullException(nameof(settingsHelper));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (cmdArgs == null)
            {
                throw new ArgumentNullException(nameof(cmdArgs));
            }

            Logging = logging;
            SettingsHelper = settingsHelper;
            Settings = settings;
            CmdArgs = cmdArgs;
        }

        public async Task<ExitCodes> RunAsync(TaskBatch batchTransformed)
        {
            // authenticate to Microsoft Identity Platform and obtain a Graph API token.
            // token is good for an hour. cache it in user env vars if the option was passed.

            var authSettings = SettingsHelper.GetMsftAuthSettings(Settings);

            Logging.WriteInfo("Auth configuration loaded.");

            string accessToken = String.Empty;

            if (CmdArgs.CacheAuthTokens)
            {
                if (!TokenCache.GetMsftTokenFromCache(out accessToken))
                {
                    Logging.WriteInfo("No recent cached token found. Will need to login.");
                }
                else
                {
                    Logging.WriteInfo("Grabbed a token from the cache.");
                }
            }

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                var res = await MsftDeviceCodeAuth.GetATokenForGraph(authSettings);

                if (res == null)
                {
                    // no need to print an error message here.
                    // an exception is thrown and caught within the token call, and is printed to the STDERR
                    // already by the time this is reached.
                    return ExitCodes.FailedToAuthenticate;
                }
                else
                {
                    Logging.WriteInfo(Environment.NewLine + "Logged in as " + res.Account.Username);
                    accessToken = res.AccessToken;

                    if (CmdArgs.CacheAuthTokens)
                    {
                        TokenCache.SaveMsftTokenToCache(res.AccessToken, DateTime.Now.AddMinutes(60));
                        Logging.WriteInfo("Token saved to the cache (valid for 1 hour).");
                    }
                }
            }

            // call the helper method that actually invokes the graph API to create the batch.

            var graphUri = Settings[Constants.MsftGraphApiBaseUri];
            var tasker = new MsftToDoApiWrapper(Logging, graphUri, accessToken, CmdArgs.TimeZone, Settings[Constants.DateFormatKeyName]);
            return await tasker.CreateBatchAsync(batchTransformed);
        }
    }
}
