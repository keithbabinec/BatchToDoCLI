namespace BatchToDoCLI.Models.Microsoft
{
    public class TodoTaskItemBody
    {
        public string content { get; set; }

        /// <summary>
        /// Accepts 'text' or 'html' values.
        /// </summary>
        public string contentType { get; set; }
    }
}
