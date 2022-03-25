using BatchToDoCLI.Models;

namespace BatchToDoCLI.Execution
{
    public interface ITaskApiWrapper
    {
        Task<ExitCodes> CreateBatchAsync(TaskBatch batchTransformed);
    }
}