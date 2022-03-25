using BatchToDoCLI;
using BatchToDoCLI.Definitions;
using BatchToDoCLI.Execution;
using BatchToDoCLI.Execution.Implementation.Microsoft;
using BatchToDoCLI.Logging;
using BatchToDoCLI.Models;

// parse command line switches.
// load the settings stored in appsettings.json

var logger = new ConsoleLogger();
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
    logger.WriteError("Required appsettings.json file is missing, or has settings that aren't filled out.");
    return (int)ExitCodes.MissingOrInvalidSettingsJson;
}

// load and parse the batch definition file provided in the arguments.
// then try to transform the definition variables with real values.

logger.WriteInfo("Loading batch definition yaml file from disk.");
TaskBatch batchDefinition = null;

try
{
    var loader = new Loader();
    batchDefinition = loader.LoadFromYaml(cmdArgs.BatchDefinition);
}
catch (Exception ex)
{
    logger.WriteError($"Batch definition file {cmdArgs.BatchDefinition} could not be loaded. It doesn't exist, wasn't accessible, or contains invalid YAML syntax. Full error below:");
    logger.WriteError(ex.ToString());
    return (int)ExitCodes.MissingOrInvalidDefinitionYaml;
}

logger.WriteInfo("Substituting batch definition variables.");
TaskBatch batchTransformed = null;

try
{
    var transformer = new Transformer();
    batchTransformed = transformer.TransformFromVariables(batchDefinition, cmdArgs.Variables);
}
catch (Exception ex)
{
    logger.WriteError($"Batch definition file {cmdArgs.BatchDefinition} could not be transformed. We ran into an error when trying to substitute variables:");
    logger.WriteError(ex.ToString());
    return (int)ExitCodes.FailedToTransformDefinitionYaml;
}

if (!cmdArgs.WhatIfMode)
{
    var commitMode = new MsftToDoTaskCreator(logger, settingsHelper, settings, cmdArgs);

    var res = await commitMode.RunAsync(batchTransformed).ConfigureAwait(false);
    return (int)res;
}
else
{
    var whatIfMode = new WhatIf(logger);

    var res = whatIfMode.Run(batchTransformed);
    return (int)res;
}