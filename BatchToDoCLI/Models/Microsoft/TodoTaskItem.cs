namespace BatchToDoCLI.Models.Microsoft
{
    public class TodoTaskItem
    {
        public string importance { get; set; }

        public bool isReminderOn { get; set; }

        public TodoTaskItemDateTime reminderDateTime { get; set; }

        public TodoTaskItemDateTime dueDateTime { get; set; }

        public string title { get; set; }

        public string status { get; set; }

        public TodoTaskItemBody body { get; set; }

        public string id { get; set; }
    }
}
