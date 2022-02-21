namespace BatchToDoCLI.Models
{
    public class TaskBatch
    {
        public string BatchName { get; set; }

        public List<TaskItem> Tasks { get; set; }
    }
}
