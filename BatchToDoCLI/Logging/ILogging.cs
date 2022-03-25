namespace BatchToDoCLI.Logging
{
    public interface ILogging
    {
        void WriteInfo(string message);

        void WriteError(string message);
    }
}
