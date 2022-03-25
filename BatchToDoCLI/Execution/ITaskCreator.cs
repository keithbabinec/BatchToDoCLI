using BatchToDoCLI.Models;
using Microsoft.Extensions.Configuration;

namespace BatchToDoCLI.Execution
{
    public interface ITaskCreator
    {
        Task<ExitCodes> RunAsync(Settings settingsHelper, IConfigurationRoot settings, CommandArguments cmdArgs, TaskBatch batchTransformed);
    }
}