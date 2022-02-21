using BatchToDoCLI.Models;
using Microsoft.Extensions.Configuration;
using YamlDotNet.Serialization;

namespace BatchToDoCLI.Execution
{
    public class WhatIf
    {
        public ExitCodes Run(Settings settingsHelper, IConfigurationRoot settings, CommandArguments cmdArgs, TaskBatch batchTransformed)
        {
            // just print out the taskbatch with substituted variables

            var serializer = new SerializerBuilder().Build();
            var yamlText = serializer.Serialize(batchTransformed);

            Console.WriteLine("Operating in -WhatIf mode. No tasks will be created. The following tasks are what would have been created." + Environment.NewLine);

            Console.WriteLine(yamlText);

            return ExitCodes.Success;
        }
    }
}
