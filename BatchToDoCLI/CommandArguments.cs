using System.Text;

namespace BatchToDoCLI
{
    public class CommandArguments
    {
        public string BatchDefinition { get; set; }

        public Variables Variables { get; set; }

        public bool CacheAuthTokens { get; set; }

        public bool WhatIfMode { get; set; }

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

            sb.AppendLine("// Usage:");
            sb.AppendLine();
            sb.AppendLine("BatchToDoCLI.exe -BatchDefinition <value> -Variables <values> [-CacheAuthTokens] [-ValidateOnly]");
            sb.AppendLine();
            sb.AppendLine("// Arguments:");
            sb.AppendLine();
            sb.AppendLine("-BatchDefinition: specify the path for a BatchToDoCLI batch definition (*.yaml)");
            sb.AppendLine();
            sb.AppendLine("-Variables: specify semicolon separated list of key-value pairs (joined with an equal-sign). Values will be placed into the batch definition.");
            sb.AppendLine();
            sb.AppendLine("-CacheAuthTokens: an optional switch to specify if auth tokens should be cached in environment variables.");
            sb.AppendLine();
            sb.AppendLine("-WhatIf: an optional switch to specify we shouldn't actually create any tasks (only print out what would be created).");
            sb.AppendLine();
            sb.AppendLine("// Examples:");
            sb.AppendLine();
            sb.AppendLine("-- prints the tasks that would be created from the batch definition (does not submit them)");
            sb.AppendLine("BatchToDoCLI.exe -BatchDefinition D:\\Definitions\\release-schedule.yaml -Variables \"RELEASEDATE=04/15/2022;SONGNAME=Survival Machines\" -WhatIf");
            sb.AppendLine();
            sb.AppendLine("-- creates the tasks provided in the batch definition (and cache the token for re-use)");
            sb.AppendLine("BatchToDoCLI.exe -BatchDefinition D:\\Definitions\\release-schedule.yaml -Variables \"RELEASEDATE=04/15/2022;SONGNAME=Survival Machines\" -CacheAuthTokens");
            sb.AppendLine();

            Console.WriteLine(sb.ToString());
        }
    }
}
