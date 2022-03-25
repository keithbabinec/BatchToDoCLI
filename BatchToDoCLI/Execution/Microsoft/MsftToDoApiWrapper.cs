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

        public MsftToDoApiWrapper(ILogging logging, string graphApiBaseUri, string accessToken)
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

            Logging = logging;
            client.BaseAddress = new Uri(graphApiBaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<ExitCodes> CreateBatchAsync(TaskBatch batchTransformed)
        {
            Logging.WriteInfo($"Checking if the task list {batchTransformed.BatchName} already exists.");

            var taskListObj = await FetchTaskListAsync(batchTransformed.BatchName).ConfigureAwait(false);

            if (taskListObj == null)
            {
                Logging.WriteInfo($"Task list does not exist. Creating it now.");

                taskListObj = await CreateTaskListAsync(batchTransformed.BatchName).ConfigureAwait(false);

                Logging.WriteInfo($"New task list created. ID: {taskListObj}");
            }
            else
            {
                Logging.WriteInfo($"Task list already exists, no need to create it. ID: {taskListObj}");
            }

            Logging.WriteInfo($"Querying for existing tasks under task list {batchTransformed.BatchName}.");

            var taskListTasks = await FetchTaskListTasks(taskListObj.id).ConfigureAwait(false);

            Logging.WriteInfo($"Found {taskListTasks.Count} total task(s) under this task list.");

            foreach (var item in batchTransformed.Tasks)
            {
                if (!taskListTasks.Any(x => String.Equals(x.title, item.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    Logging.WriteInfo($"Task '{item.Name}' doesn't already exist, creating it now.");
                    
                    string newTaskId = await CreateTaskAsync(item);

                    Logging.WriteInfo($"Task created successfully. ID: {newTaskId}");
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
            var stream = await client.GetStreamAsync($"me/todo/lists").ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<TodoTaskListsResult>(stream).ConfigureAwait(false);

            if (result == null || result.value == null || result.value.Count == 0)
            {
                return null;
            }
            else
            {
                return result.value.FirstOrDefault(x => String.Equals(x.displayName, taskListName, StringComparison.OrdinalIgnoreCase));
            }
        }

        private async Task<TodoTaskList> CreateTaskListAsync(string taskListName)
        {
            // POST /me/todo/lists
            // Content-Type	application/json
            /*
            {
                "displayName": "Travel items"
            }
            // returns 
            {
              "@odata.type": "#microsoft.graph.todoTaskList",
              "id": "AAMkADIyAAAhrbPWAAA=",
              "displayName": "Travel items",
              "isOwner": true,
              "isShared": false,
              "wellknownListName": "none"
            }
             */
        }

        private async Task<List<TodoTaskItem>> FetchTaskListTasks(string taskListId)
        {
            var stream = await client.GetStreamAsync($"me/todo/lists/{taskListId}/tasks").ConfigureAwait(false);
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

        private async Task<string> CreateTaskAsync(TaskItem item)
        {
            // POST /me/todo/lists/{todoTaskListId}/tasks
            // Content-Type	application/json
            // body: https://docs.microsoft.com/en-us/graph/api/todotasklist-post-tasks?view=graph-rest-1.0&tabs=http#request-body
            /* response:
             {
               "@odata.etag":"W/\"xzyPKP0BiUGgld+lMKXwbQAAnBoTIw==\"",
               "importance":"low",
               "isReminderOn":false,
               "status":"notStarted",
               "title":"A new task",
               "createdDateTime":"2020-08-18T09:03:05.8339192Z",
               "lastModifiedDateTime":"2020-08-18T09:03:06.0827766Z",
               "id":"AlMKXwbQAAAJws6wcAAAA=",
               "body":{
                  "content":"",
                  "contentType":"text"
               },
               "linkedResources":[
                  {
                     "id":"f9cddce2-dce2-f9cd-e2dc-cdf9e2dccdf9",
                     "webUrl":"http://microsoft.com",
                     "applicationName":"Microsoft",
                     "displayName":"Microsoft"
                  }
               ]
            }
             */
        }
    }
}
