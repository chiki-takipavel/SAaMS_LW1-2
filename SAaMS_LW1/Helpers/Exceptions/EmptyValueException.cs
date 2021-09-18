using System;
using System.Runtime.Serialization;

namespace SAaMS_LW1.Helpers.Exceptions
{
    [Serializable]
    public class EmptyValueException : Exception
    {
        public EmptyValueException()
        {
        }

        public EmptyValueException(string message) : base(message)
        {
        }

        public EmptyValueException(string message, Exception inner) : base(message, inner)
        {
        }

        protected EmptyValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
