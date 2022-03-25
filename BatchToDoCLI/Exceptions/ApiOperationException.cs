using System.Runtime.Serialization;

namespace BatchToDoCLI.Exceptions
{
    [Serializable]
    internal class ApiOperationException : Exception
    {
        public ApiOperationException()
        {
        }

        public ApiOperationException(string message) : base(message)
        {
        }

        public ApiOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApiOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
