namespace BatchToDoCLI.Logging
{
    public class ConsoleLogger : ILogging
    {
        private const string DateFormat = "yyyy-MM-dd HH:ss";

        public void WriteError(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString(DateFormat)}: {message}");
        }

        public void WriteInfo(string message)
        {
            Console.Error.WriteLine($"{DateTime.Now.ToString(DateFormat)}: {message}");
        }
    }
}
