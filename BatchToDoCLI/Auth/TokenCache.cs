namespace BatchToDoCLI.Auth
{
    public class TokenCache
    {
        public static bool GetMsftTokenFromCache(out string TokenValue)
        {
            TokenValue = String.Empty;

            var cacheDateStr = Environment.GetEnvironmentVariable(Constants.MsftTokenCacheDateKeyName, EnvironmentVariableTarget.User);
            var cacheValue = Environment.GetEnvironmentVariable(Constants.MsftTokenCacheValueKeyName, EnvironmentVariableTarget.User);

            if (string.IsNullOrEmpty(cacheDateStr) || string.IsNullOrEmpty(cacheValue))
            {
                // no cache data.
                return false;
            }

            DateTime expires;
            if (!DateTime.TryParse(cacheDateStr, out expires))
            {
                // invalid token cache data.
                return false;
            }

            if (DateTime.Now > expires.AddMinutes(-10))
            {
                // token is expired, or expires in the next 10 minutes?
                return false;
            }

            TokenValue = cacheValue;

            return true;
        }

        public static void SaveMsftTokenToCache(string TokenValue, DateTime TokenExpires)
        {
            Environment.SetEnvironmentVariable(Constants.MsftTokenCacheValueKeyName, TokenValue, EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable(Constants.MsftTokenCacheDateKeyName, TokenExpires.ToString(), EnvironmentVariableTarget.User);
        }
    }
}
