using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;

namespace PakAnas.Commons.Exceptions
{
    [Serializable]
    public class DBConnectionException : Exception
    {
        public DBConnectionException()
            : base() { }

        public DBConnectionException(string message)
            : base(message) { }

        public DBConnectionException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public DBConnectionException(string message, Exception innerException)
            : base(message, innerException) { }

        public DBConnectionException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected DBConnectionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }


        private static string[] messages = new string[] 
        {
            "A network-related or instance-specific error occurred while establishing a connection to SQL Server",
            "A transport-level error has occurred when sending the request to the server", //232
            "A transport-level error has occurred when receiving results from the server",
            "The connection's current state is closed",
            "The connection is closed"
        };

        private static int[] numbers = new int[] { 
            -2, //Timeout expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.
            2, //An error has occurred while establishing a connection to the server. When connecting to SQL Server, this failure may be caused by the fact that under the default settings SQL Server does not allow remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server ) (.Net SqlClient Data Provider)
            53, //An error has occurred while establishing a connection to the server. When connecting to SQL Server, this failure may be caused by the fact that under the default settings SQL Server does not allow remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server ) (.Net SqlClient Data Provider).
            232, //A transport-level error has occurred when sending the request to the server.
            233, //A connection was successfully established with the server, but then an error occurred during the login process. (provider: Shared Memory Provider, error: 0 - No process is on the other end of the pipe.) (Microsoft SQL Server, Error: 233)
            4060, //Cannot open database "RENEWAL_IKBP" requested by the login. The login failed.
        };

        ///// <summary>
        /////     Check whether Exception is DB Connection Exception or not
        ///// </summary>
        ///// <param name="ex">the exception need to be check</param>
        ///// <returns>if return null then it is not DB Connection Exception</returns>
        //public static DBConnectionException IsDBConnectionException(Exception ex)
        //{
        //    if (ex == null) 
        //    {
        //        return null;
        //    }

        //    if (ex is SqlException &&
        //        messages.Any(s => ex.Message.Contains(s))) 
        //    {
        //        Console.WriteLine("ErrorCode = " + ((SqlException)ex).ErrorCode);
        //        Console.WriteLine(ex);
        //        return new DBConnectionException("DB Connection Error occur", ex);
        //    }

        //    return null;
        //}

        /// <summary>
        ///     Check whether Exception is DB Connection Exception or not
        /// </summary>
        /// <param name="ex">the exception need to be check</param>
        /// <returns>if return null then it is not DB Connection Exception</returns>
        public static DBConnectionException IsDBConnectionException(Exception ex)
        {
            if (ex == null)
            {
                return null;
            }

            if (ex is SqlException &&
                numbers.Any(s => ((SqlException)ex).Number == s))
            {
                return new DBConnectionException("DB Connection Error occur", ex);
            }

            if (messages.Any(s => ex.Message.Contains(s)))
            {
                return new DBConnectionException("DB Connection Error occur", ex);
            }

            return null;
        }

        /// <summary>
        ///     Check whether Exception is DB Connection Exception or not
        ///     Throw DBConnectionException if true
        /// </summary>
        /// <param name="ex">the exception need to be check</param>
        public static void ThrowDBConnectionExceptionIfTrue(Exception ex) 
        {
            Exception ex2 = IsDBConnectionException(ex);
            if (ex2 != null)
            {
                throw ex2;
            }        
        }
    }
}