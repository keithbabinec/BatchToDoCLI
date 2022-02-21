using System.Text;

namespace BatchToDoCLI
{
    public class CommandArguments
    {
        public string BatchDefinition { get; set; }

        public Variables Variables { get; set; }

        public bool CacheAuthTokens { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(BatchDefinition))
            {
                return false;
            }
            if (Variables.Count == 0)
            {
                return false;
            }

            return true;
        }

        public static void DisplayHelp()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Usage:");
            sb.AppendLine();
            sb.AppendLine("BatchToDoCLI.exe -BatchDefinition <value> -Variables <values> [-CacheAuthTokens]");
            sb.AppendLine();
            sb.AppendLine("Arguments:");
            sb.AppendLine();
            sb.AppendLine("-BatchDefinition: specify the path for a BatchToDoCLI batch definition (*.yaml)");
            sb.AppendLine("-Variables: specify semicolon separated list of key-value pairs (joined with an equal-sign). Values will be placed into the batch definition.");
            sb.AppendLine("-CacheAuthTokens: an optional switch to specify if auth tokens should be cached in environment variables.");
            sb.AppendLine();
            sb.AppendLine("Examples:");
            sb.AppendLine();
            sb.AppendLine("BatchToDoCLI.exe -BatchDefinition D:\\Definitions\\release-schedule.yaml -Variables \"RELEASEDATE=04/15/2022;SONGNAME=Survival Machines\" -CacheAuthTokens");
            sb.AppendLine();

            Console.WriteLine(sb.ToString());
        }
    }
}
