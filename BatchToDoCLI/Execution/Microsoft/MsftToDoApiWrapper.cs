using BatchToDoCLI.Logging;
using BatchToDoCLI.Models;
using BatchToDoCLI.Models.Microsoft;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BatchToDoCLI.Execution.Microsoft
{
    public class MsftToDoApiWrapper : ITaskApiWrapper
    {
        private static HttpClient client = new HttpClient();

        private ILogging Logging;

        private string Timezone;

        private string DateFormat;

        public MsftToDoApiWrapper(ILogging logging, string graphApiBaseUri, string accessToken, string timeZone, string dateFormat)
        {
            if (logging is null)
            {
                throw new ArgumentNullException(nameof(logging));
            }
            if (string.IsNullOrWhiteSpace(graphApiBaseUri))
            {
                throw new ArgumentException(nameof(graphApiBaseUri) + " must be provided.");
            }
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException(nameof(accessToken) + " must be provided.");
            }
            if (string.IsNullOrWhiteSpace(timeZone))
            {
                throw new ArgumentException($"'{nameof(timeZone)}' cannot be null or whitespace.");
            }
            if (string.IsNullOrWhiteSpace(dateFormat))
            {
                throw new ArgumentException($"'{nameof(dateFormat)}' cannot be null or whitespace.");
            }

            Timezone = timeZone;
            DateFormat = dateFormat;
            Logging = logging;
            client.BaseAddress = new Uri(graphApiBaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<ExitCodes> CreateBatchAsync(TaskBatch batchTransformed)
        {
            Logging.WriteInfo($"Checking if the task list '{batchTransformed.BatchName}' already exists.");

            var taskListObj = await FetchTaskListAsync(batchTransformed.BatchName).ConfigureAwait(false);

            if (taskListObj == null)
            {
                Logging.WriteInfo($"Task list does not exist. Creating it now.");

                taskListObj = await CreateTaskListAsync(batchTransformed.BatchName).ConfigureAwait(false);

                Logging.WriteInfo($"New task list created.");
            }
            else
            {
                Logging.WriteInfo($"Task list already exists, no need to create it.");
            }

            Logging.WriteInfo($"Querying for existing tasks under task list '{batchTransformed.BatchName}'.");

            var taskListTasks = await FetchTaskListTasks(taskListObj.id).ConfigureAwait(false);

            Logging.WriteInfo($"Found {taskListTasks.Count} total task(s) under this task list.");

            for (int i = batchTransformed.Tasks.Count - 1; i >= 0; i--)
            {
                // create the tasks in backwards order.
                // otherwise the list will be backwards when viewing in To-Do.

                var item = batchTransformed.Tasks[i];

                if (!taskListTasks.Any(x => String.Equals(x.title, item.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    Logging.WriteInfo($"Task '{item.Name}' doesn't already exist, creating it now.");
                    
                    var newTask = await CreateTaskAsync(taskListObj.id, item).ConfigureAwait(false);

                    Logging.WriteInfo($"Task created successfully.");
                }
                else
                {
                    Logging.WriteInfo($"Task '{item.Name}' already exists, no need to create it.");
                }
            }

            Logging.WriteInfo("Finished processing all tasks.");

            return ExitCodes.Success;
        }

        private async Task<TodoTaskList> FetchTaskListAsync(string taskListName)
        {
            using (var stream = await client.GetStreamAsync($"me/todo/lists").ConfigureAwait(false))
            {
                var result = await JsonSerializer.DeserializeAsync<TodoTaskListsResult>(stream).ConfigureAwait(false);

                if (result == null || result.value == null || result.value.Count == 0)
                {
                    return null;
                }
                else
                {
                    return result.value.FirstOrDefault(x => string.Equals(x.displayName, taskListName, StringComparison.OrdinalIgnoreCase));
                }
            }
        }

        private async Task<TodoTaskList> CreateTaskListAsync(string taskListName)
        {
            var tl = new TodoTaskListPostModel(taskListName);
            var serialized = JsonSerializer.Serialize(tl, tl.GetType());

            var httpContent = new StringContent(serialized, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("me/todo/lists", httpContent).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObj = JsonSerializer.Deserialize<TodoTaskList>(responseContent);

            return responseObj;
        }

        private async Task<List<TodoTaskItem>> FetchTaskListTasks(string taskListId)
        {
            using (var stream = await client.GetStreamAsync($"me/todo/lists/{taskListId}/tasks").ConfigureAwait(false))
            {
                var result = await JsonSerializer.DeserializeAsync<TodoTaskListTasksResult>(stream).ConfigureAwait(false);

                if (result == null || result.value == null || result.value.Count == 0)
                {
                    return new List<TodoTaskItem>();
                }
                else
                {
                    return result.value;
                }
            }   
        }

        private async Task<TodoTaskItem> CreateTaskAsync(string taskListId, TaskItem item)
        {
            var taskItem = new TodoTaskItemPostModel(item, Timezone, DateFormat);
            var serialized = JsonSerializer.Serialize(taskItem, taskItem.GetType());

            var httpContent = new StringContent(serialized, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"me/todo/lists/{taskListId}/tasks", httpContent).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObj = JsonSerializer.Deserialize<TodoTaskItem>(responseContent);

            return responseObj;
        }
    }
}
