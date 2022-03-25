using BatchToDoCLI.Logging;
using BatchToDoCLI.Models;
using Microsoft.Extensions.Configuration;

namespace BatchToDoCLI.Execution
{
    public interface ITaskCreator
    {
        Task<ExitCodes> RunAsync(TaskBatch batchTransformed);
    }
}