using BatchToDoCLI.Models;
using System.Net.Http.Headers;

namespace BatchToDoCLI.Execution.Implementation.Microsoft
{
    public class MsftToDoApiWrapper : ITaskApiWrapper
    {
        private static HttpClient client = new HttpClient();

        public MsftToDoApiWrapper(string graphApiBaseUri, string accessToken)
        {
            if (string.IsNullOrWhiteSpace(graphApiBaseUri))
            {
                throw new ArgumentException(nameof(graphApiBaseUri) + " must be provided.");
            }

            client.BaseAddress = new Uri(graphApiBaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ExitCodes> CreateBatchAsync(TaskBatch batchTransformed)
        {
            throw new NotImplementedException();
        }
    }
}
