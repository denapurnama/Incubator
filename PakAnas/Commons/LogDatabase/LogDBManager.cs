using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toyota.Common.Database;

namespace PakAnas.Commons.LogDatabase
{
    class LogDBManager
    {
        private static LogDBManager instance;

        public static LogDBManager Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new LogDBManager();
                }

                return instance;
            }
        }

        private LogDBManager() { }

        public ILogDBSession CreateSession(string userId, int moduleId, int functionId)
        {
            return new LogDBSession(userId, moduleId, functionId, null);
        }

        public ILogDBSession CreateSession(string userId, int moduleId, int functionId, IDBContext dbContext)
        {
            return new LogDBSession(userId, moduleId, functionId, dbContext);
        }
    }
}
