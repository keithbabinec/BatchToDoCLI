namespace BatchToDoCLI.Models.Microsoft
{
    public class TodoTaskListPostModel
    {
        public TodoTaskListPostModel()
        {
        }

        public TodoTaskListPostModel(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.");
            }

            displayName = name;
        }

        public string displayName { get; set; }
    }
}
