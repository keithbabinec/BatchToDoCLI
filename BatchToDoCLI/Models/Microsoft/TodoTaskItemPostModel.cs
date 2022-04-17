namespace BatchToDoCLI.Models.Microsoft
{
    public class TodoTaskItemPostModel
    {
        public TodoTaskItemPostModel()
        {
        }

        public TodoTaskItemPostModel(TaskItem item, string timeZone, string dateFormat)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            title = item.Name;
            body = new TodoTaskItemBody()
            {
                contentType = "html",
                content = item.Description
            };

            var dueDate = DateTime.ParseExact(item.Evaluated, dateFormat, null);

            dueDateTime = new TodoTaskItemDateTime()
            {
                dateTime = dueDate.ToString("s"),
                timeZone = timeZone
            };
        }

        public string importance { get; set; }

        public bool isReminderOn { get; set; }

        public TodoTaskItemDateTime reminderDateTime { get; set; }

        public TodoTaskItemDateTime dueDateTime { get; set; }

        public string title { get; set; }

        public TodoTaskItemBody body { get; set; }
    }
}
