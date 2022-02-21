using BatchToDoCLI;
using BatchToDoCLI.Definitions;
using BatchToDoCLI.Models;

// parse command line switches.
// load the settings stored in appsettings.json

var settingsHelper = new Settings();
var cmdArgs = settingsHelper.ParseCommandArguments(args);
var settings = settingsHelper.LoadAppSettings();

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

// load and parse the batch definition file provided in the arguments.
// then try to transform the definition variables with real values.

Console.WriteLine("Loading batch definition yaml file from disk.");
TaskBatch batchDefinition = null;

try
{
    var loader = new Loader();
    batchDefinition = loader.LoadFromYaml(cmdArgs.BatchDefinition);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Batch definition file {cmdArgs.BatchDefinition} could not be loaded. It doesn't exist, wasn't accessible, or contains invalid YAML syntax. Full error below:");
    Console.Error.WriteLine(ex.ToString());
    return (int)ExitCodes.MissingOrInvalidDefinitionYaml;
}


Console.WriteLine("Substituting batch definition variables.");
TaskBatch batchTransformed = null;

try
{
    var transformer = new Transformer();
    batchTransformed = transformer.TransformFromVariables(batchDefinition, cmdArgs.Variables);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Batch definition file {cmdArgs.BatchDefinition} could not be transformed. We ran into an error when trying to substitute variables:");
    Console.Error.WriteLine(ex.ToString());
    return (int)ExitCodes.FailedToTransformDefinitionYaml;
}

// authenticate to Microsoft Identity Platform and obtain a Graph API token.
// token is good for an hour. cache it in user env vars if the option was passed.

var authSettings = settingsHelper.GetAuthSettings(settings);

Console.WriteLine("Auth configuration loaded.");

string accessToken = String.Empty;

//if (cmdArgs.CacheAuthTokens)
//{
//    if (!TokenCache.GetTokenFromCache(out accessToken))
//    {
//        Console.WriteLine("No recent cached token found. Will need to login.");
//    }
//    else
//    {
//        Console.WriteLine("Grabbed a token from the cache.");
//    }
//}

//if (string.IsNullOrWhiteSpace(accessToken))
//{
//    var res = await DeviceCodeAuth.GetATokenForGraph(authSettings);

//    if (res == null)
//    {
//        // no need to print an error message here.
//        // an exception is thrown and caught within the token call, and is printed to the STDERR
//        // already by the time this is reached.
//        return (int)ExitCodes.FailedToAuthenticate;
//    }
//    else
//    {
//        Console.WriteLine(Environment.NewLine + "Logged in as " + res.Account.Username);

//        if (cmdArgs.CacheAuthTokens)
//        {
//            TokenCache.SaveTokenToCache(res.AccessToken, DateTime.Now.AddMinutes(60));
//            Console.WriteLine("Token saved to the cache (valid for 1 hour).");
//        }
//    }
//}


return (int)ExitCodes.Success;