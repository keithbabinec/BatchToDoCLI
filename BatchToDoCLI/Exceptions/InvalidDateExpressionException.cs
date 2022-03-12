using System.Runtime.Serialization;

namespace BatchToDoCLI.Exceptions
{
    [Serializable]
    internal class InvalidDateExpressionException : Exception
    {
        public InvalidDateExpressionException()
        {
        }

        public InvalidDateExpressionException(string message) : base(message)
        {
        }

        public InvalidDateExpressionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidDateExpressionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}