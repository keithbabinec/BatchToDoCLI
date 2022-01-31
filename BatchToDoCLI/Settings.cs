using BatchToDoCLI.Auth;
using Microsoft.Extensions.Configuration;

namespace BatchToDoCLI
{
    public class Settings
    {
        public static IConfigurationRoot LoadAppSettings()
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

        public static AuthSettings GetAuthSettings(IConfigurationRoot settings)
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
