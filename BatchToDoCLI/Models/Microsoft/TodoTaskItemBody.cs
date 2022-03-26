namespace BatchToDoCLI.Models.Microsoft
{
    public class TodoTaskItemBody
    {
        public string content;

        /// <summary>
        /// Accepts 'text' or 'html' values.
        /// </summary>
        public string contentType;
    }
}
