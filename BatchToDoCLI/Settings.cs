using BatchToDoCLI.Auth;
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
                    case "-cacheauthtokens":
                        {
                            cmdArgs.CacheAuthTokens = true;
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
            if (string.IsNullOrEmpty(appConfig[Constants.AppIdSettingName]) ||
                string.IsNullOrEmpty(appConfig[Constants.ScopeSettingName]) ||
                string.IsNullOrEmpty(appConfig[Constants.AuthoritySettingName]))
            {
                return null;
            }

            return appConfig;
        }

        public AuthSettings GetAuthSettings(IConfigurationRoot settings)
        {
            return new AuthSettings()
            {
                AppId = settings[Constants.AppIdSettingName],
                Scopes = settings[Constants.ScopeSettingName].Split(';'),
                Authority = settings[Constants.AuthoritySettingName]
            };
        }
    }
}
