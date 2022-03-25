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
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException(nameof(accessToken) + " must be provided.");
            }

            client.BaseAddress = new Uri(graphApiBaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<ExitCodes> CreateBatchAsync(TaskBatch batchTransformed)
        {
            var taskListId = await FetchTaskListIdAsync(batchTransformed.BatchName).ConfigureAwait(false);

            if (taskListId == null)
            {
                taskListId = await CreateTaskListAsync(batchTransformed.BatchName).ConfigureAwait(false);
            }

            var taskNames = await FetchTaskListTaskNames(taskListId).ConfigureAwait(false);

            foreach (var item in batchTransformed.Tasks)
            {
                if (!taskNames.Any(x => x == item.Name))
                {
                    string z = await CreateTaskAsync(item);
                }
                else
                {
                    // already created.
                }
            }

            return ExitCodes.Success;
        }

        private async Task<string> FetchTaskListIdAsync(string taskListName)
        {
            // GET /me/todo/lists
            /*
             {
                "value": [
                {
                    "@odata.type": "#microsoft.graph.todoTaskList",
                    "id": "AAMkADIyAAAAABrJAAA=",
                    "displayName": "Tasks",
                    "isOwner": true,
                    "isShared": false,
                    "wellknownListName": "defaultList"
                }
                ]
            }
             */
        }

        private async Task<string> CreateTaskListAsync(string taskListName)
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

        private async Task<List<string>> FetchTaskListTaskNames(string taskListId)
        {
            // GET /me/todo/lists/{todoTaskListId}/tasks
            /*
             {
                "value":[
                    {
                        "@odata.etag":"W/\"xzyPKP0BiUGgld+lMKXwbQAAgdhkVw==\"",
                        "importance":"low",
                        "isReminderOn":false,
                        "status":"notStarted",
                        "title":"Linked entity new task 1",
                        "createdDateTime":"2020-07-08T11:15:19.9359889Z",
                        "lastModifiedDateTime":"2020-07-08T11:15:20.0614375Z",
                        "id":"AQMkADAwATM0MDAAMS0yMDkyLWVjMzYtMDACLTAwCgBGAAAD",
                        "body":{
                        "content":"",
                        "contentType":"text"
                        },
                        "linkedResources@odata.context":"https://graph.microsoft.com/beta/$metadata#users('todoservicetest2412201901%40outlook.com')/todo/lists('35e2-35e2-721a-e235-1a72e2351a7')/tasks('AQMkADAwATM0MDAAMS0yMDkyLWVjMzYtMDACLTAwCgBGAAAD')/linkedResources",
                        "linkedResources":[
                        {
                            "applicationName":"Partner App Name",
                            "displayName":"Partner App Name",
                            "externalId":"teset1243434",
                            "id":"30911960-7321-4cba-9ba0-cdb68e2984c7"
                        }
                        ]
                    }
                ]
            }
             */
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
