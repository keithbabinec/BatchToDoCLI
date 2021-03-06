using BatchToDoCLI.Auth.Microsoft;
using Microsoft.Extensions.Configuration;

namespace BatchToDoCLI
{
    public class Settings
    {
        public CommandArguments ParseCommandArguments(string[] args)
        {
            var cmdArgs = new CommandArguments();

            int argPosition = 0;

            string argType = string.Empty;

            while (argPosition < args.Length)
            {
                var current = args[argPosition];
                var currentIsValue = false;

                switch (current.ToLower())
                {
                    case "-batchdefinition":
                        {
                            argType = nameof(CommandArguments.BatchDefinition);
                            break;
                        }
                    case "-variables":
                        {
                            argType = nameof(CommandArguments.Variables);
                            break;
                        }
                    case "-timezone":
                        {
                            argType = nameof(CommandArguments.TimeZone);
                            break;
                        }
                    case "-cacheauthtokens":
                        {
                            cmdArgs.CacheAuthTokens = true;
                            break;
                        }
                    case "-whatif":
                        {
                            cmdArgs.WhatIfMode = true;
                            break;
                        }
                    default:
                        {
                            currentIsValue = true;
                            break;
                        }
                }

                if (currentIsValue)
                {
                    switch (argType)
                    {
                        case nameof(CommandArguments.BatchDefinition):
                            {
                                cmdArgs.BatchDefinition = current;
                                break;
                            }
                        case nameof(CommandArguments.Variables):
                            {
                                cmdArgs.Variables = new Variables(current);
                                break;
                            }
                        case nameof(CommandArguments.TimeZone):
                            {
                                cmdArgs.TimeZone = current;
                                break;
                            }
                    }
                }

                argPosition++;
            }

            if (cmdArgs.Validate())
            {
                return cmdArgs;
            }
            else
            {
                return null;
            }
        }

        public IConfigurationRoot LoadAppSettings()
        {
            var fileName = "";

            if (File.Exists(Constants.DevSettingsFileName))
            {
                fileName = Constants.DevSettingsFileName;
            }
            else
            {
                fileName = Constants.SettingsFileName;
            }

            var appConfig = new ConfigurationBuilder()
                .AddJsonFile(fileName)
                .Build();

            // Check for required settings
            if (string.IsNullOrEmpty(appConfig[Constants.DateFormatKeyName]) || 
                string.IsNullOrEmpty(appConfig[Constants.MsftGraphApiBaseUri]) || 
                string.IsNullOrEmpty(appConfig[Constants.MsftAppIdSettingName]) ||
                string.IsNullOrEmpty(appConfig[Constants.MsftScopeSettingName]) ||
                string.IsNullOrEmpty(appConfig[Constants.MsftAuthoritySettingName]))
            {
                return null;
            }

            return appConfig;
        }

        public MsftAuthSettings GetMsftAuthSettings(IConfigurationRoot settings)
        {
            return new MsftAuthSettings()
            {
                AppId = settings[Constants.MsftAppIdSettingName],
                Scopes = settings[Constants.MsftScopeSettingName].Split(';'),
                Authority = settings[Constants.MsftAuthoritySettingName]
            };
        }
    }
}
