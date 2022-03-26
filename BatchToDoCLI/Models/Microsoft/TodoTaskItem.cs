namespace BatchToDoCLI.Models.Microsoft
{
    public class TodoTaskItem
    {
        public TodoTaskItem()
        {
        }

        public TodoTaskItem(TaskItem item, string timeZone, string dateFormat)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            title = item.Name;
            body = new TodoTaskItemBody()
            {
                contentType = "text",
                content = item.Description
            };

            var dueDate = DateTime.ParseExact(item.DueDate, dateFormat, null);

            dueDateTime = new TodoTaskItemDateTime()
            {
                dateTime = dueDate.ToString("s"),
                timeZone = timeZone
            };
        }

        public string importance;

        public bool isReminderOn;

        public TodoTaskItemDateTime reminderDateTime;

        public TodoTaskItemDateTime dueDateTime;

        public string title;

        public string status;

        public TodoTaskItemBody body;

        public string id;
    }
}
