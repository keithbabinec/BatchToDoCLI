namespace BatchToDoCLI.Auth.Microsoft
{
    public class MsftAuthSettings
    {
        public string AppId { get; set; }

        public string[] Scopes { get; set; }
        
        public string Authority { get; set; }
    }
}
