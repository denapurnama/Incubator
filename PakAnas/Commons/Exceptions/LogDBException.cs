using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PakAnas.Commons.Exceptions
{
    [Serializable]
    public class LogDBException : Exception
    {
        public LogDBException()
            : base() { }

        public LogDBException(string message)
            : base(message) { }

        public LogDBException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public LogDBException(string message, Exception innerException)
            : base(message, innerException) { }

        public LogDBException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected LogDBException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}