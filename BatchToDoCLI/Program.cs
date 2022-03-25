using BatchToDoCLI;
using BatchToDoCLI.Definitions;
using BatchToDoCLI.Execution;
using BatchToDoCLI.Execution.Implementation.Microsoft;
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

if (!cmdArgs.WhatIfMode)
{
    var commitMode = new MsftToDoTaskCreator();

    var res = await commitMode.RunAsync(settingsHelper, settings, cmdArgs, batchTransformed).ConfigureAwait(false);
    return (int)res;
}
else
{
    var whatIfMode = new WhatIf();

    var res = whatIfMode.Run(settingsHelper, settings, cmdArgs, batchTransformed);
    return (int)res;
}