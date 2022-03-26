using System.Text;

namespace BatchToDoCLI
{
    public class CommandArguments
    {
        public string BatchDefinition { get; set; }

        public Variables Variables { get; set; }

        public bool CacheAuthTokens { get; set; }

        public bool WhatIfMode { get; set; }

        public string TimeZone { get; set; }

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
            if (TimeZone == null)
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
            sb.AppendLine("-Timezone: specify the timezone you want the tasks to be created in. For an exact list of accepted timezone names, run 'tzutil /l' on Windows, or visit https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-time-zones?view=windows-11");
            sb.AppendLine();
            sb.AppendLine("-CacheAuthTokens: an optional switch to specify if auth tokens should be cached in environment variables.");
            sb.AppendLine();
            sb.AppendLine("-WhatIf: an optional switch to specify we shouldn't actually create any tasks (only print out what would be created).");
            sb.AppendLine();
            sb.AppendLine("// Examples:");
            sb.AppendLine();
            sb.AppendLine("-- prints the tasks that would be created from the batch definition (does not submit them)");
            sb.AppendLine("BatchToDoCLI.exe -BatchDefinition D:\\Definitions\\release-schedule.yaml -Variables \"RELEASEDATE=04/15/2022;SONGNAME=Survival Machines\" -Timezone 'Pacific Standard Time' -WhatIf");
            sb.AppendLine();
            sb.AppendLine("-- creates the tasks provided in the batch definition (and cache the token for re-use)");
            sb.AppendLine("BatchToDoCLI.exe -BatchDefinition D:\\Definitions\\release-schedule.yaml -Variables \"RELEASEDATE=04/15/2022;SONGNAME=Survival Machines\" -Timezone 'W. Europe Standard Time' -CacheAuthTokens");
            sb.AppendLine();

            Console.WriteLine(sb.ToString());
        }
    }
}
