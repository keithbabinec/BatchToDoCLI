using BatchToDoCLI;
using BatchToDoCLI.Auth;
using Microsoft.Graph;

// load the app settings

var settings = Settings.LoadAppSettings();

if (settings == null)
{
    Console.Error.WriteLine("Required appsettings.json file is missing, or has settings that aren't filled out.");
}

var authSettings = Settings.GetAuthSettings(settings);

Console.WriteLine("Configuration loaded.");

// authenticate (device code)

//var res = await DeviceCodeAuth.GetATokenForGraph(authSettings);

//if (res == null)
//{
//    return;
//}
//else
//{
//    Console.WriteLine("Logged in as " + res.Account.Username);
//    Console.WriteLine("Token: " + res.AccessToken);
//}

// authenticate (interactive)

var res = await InteractiveAuth.AcquireToken(authSettings);

if (res == null)
{
    return;
}
else
{
    Console.WriteLine("Logged in as " + res.Account.Username);
    Console.WriteLine("Token: " + res.AccessToken);
}

// perform application functions

