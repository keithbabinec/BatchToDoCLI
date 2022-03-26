using BatchToDoCLI.Logging;
using BatchToDoCLI.Models;
using YamlDotNet.Serialization;

namespace BatchToDoCLI.Execution
{
    public class WhatIf
    {
        private ILogging Logging;

        public WhatIf(ILogging logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            Logging = logger;
        }

        public ExitCodes Run(TaskBatch batchTransformed)
        {
            // just print out the taskbatch with substituted variables

            var serializer = new SerializerBuilder().Build();
            var yamlText = serializer.Serialize(batchTransformed);

            Logging.WriteInfo("Operating in -WhatIf mode. No tasks will be created. The following tasks are what would have been created.");
            Logging.WriteInfo(yamlText);

            return ExitCodes.Success;
        }
    }
}
