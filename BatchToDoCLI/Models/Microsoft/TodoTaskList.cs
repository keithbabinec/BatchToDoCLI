namespace BatchToDoCLI.Models.Microsoft
{
    public class TodoTaskList
    {
        public TodoTaskList()
        {
        }

        public TodoTaskList(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.");
            }

            displayName = name;
        }

        public string id;

        public string displayName;
    }
}
