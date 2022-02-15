using BatchToDoCLI;
using BatchToDoCLI.Auth;

// parse command line switches.
// load the settings stored in appsettings.json

var cmdArgs = Settings.ParseCommandArguments(args);
var settings = Settings.LoadAppSettings();

if (cmdArgs == null)
{
    CommandArguments.DisplayHelp();
    return (int)ExitCodes.InvalidArguments;
}

if (settings == null)
{
    Console.Error.WriteLine("Required appsettings.json file is missing, or has settings that aren't filled out.");
    return (int)ExitCodes.MissingOrInvalidSettingsJson;
}

var authSettings = Settings.GetAuthSettings(settings);

Console.WriteLine("Configuration loaded.");

// authenticate to Microsoft Identity Platform and obtain a Graph API token.
// token is good for an hour. cache it in user env vars if the option was passed.

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
        return (int)ExitCodes.FailedToAuthenticate;
    }
    else
    {
        Console.WriteLine(Environment.NewLine + "Logged in as " + res.Account.Username);

        if (cmdArgs.CacheAuthTokens)
        {
            TokenCache.SaveTokenToCache(res.AccessToken, DateTime.Now.AddMinutes(60));
            Console.WriteLine("Token saved to the cache (valid for 1 hour).");
        }
    }
}


return (int)ExitCodes.Success;