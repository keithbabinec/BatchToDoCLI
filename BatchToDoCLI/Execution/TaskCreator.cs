using BatchToDoCLI.Models;
using System.Net.Http.Headers;

namespace BatchToDoCLI.Execution
{
    public class TaskCreator
    {
        private static HttpClient client = new HttpClient();

        public TaskCreator(string graphApiBaseUri)
        {
            if (string.IsNullOrWhiteSpace(graphApiBaseUri))
            {
                throw new ArgumentException(nameof(graphApiBaseUri) + " must be provided.");
            }

            client.BaseAddress = new Uri(graphApiBaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ExitCodes> CreateBatchAsync(string accessToken, TaskBatch batchTransformed)
        {
            throw new NotImplementedException();
        }
    }
}
