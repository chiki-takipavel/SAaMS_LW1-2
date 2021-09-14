using System;
using System.Runtime.Serialization;

namespace SAaMS_LW1.Helpers.Exceptions
{
    [Serializable]
    public class ShowValuesException : Exception
    {
        public ShowValuesException()
        {
        }

        public ShowValuesException(string message) : base(message)
        {
        }

        public ShowValuesException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ShowValuesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
