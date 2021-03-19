using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toyota.Common.Database;
using System.Data.SqlClient;
using PakAnas.Commons.Repositories;
using PakAnas.Commons.Exceptions;
using Toyota.Common.Web.Platform;


namespace PakAnas.Commons.LogDatabase
{
    public class LogDBSession : BaseRepo, ILogDBSession
    {
        private readonly static string PROCESS_STS_SUCCESS = "0";
        private readonly static string PROCESS_STS_ERROR_BUSINESS = "1";
        private readonly static string PROCESS_STS_ERROR_SYSTEM = "2";
        //private readonly static string PROCESS_STS_WARNING = "3";
        private readonly static string PROCESS_STS_IN_PROGRESS = "4";

        private readonly static string LOG_RESULT_SUCCESS = "TRUE";
        private readonly static string LOG_RESULT_ERROR = "FALSE";

        private readonly static string MESG_TYPE_ERROR = "ERR";
        private readonly static string MESG_TYPE_INFORMATION = "INF";
        private readonly static string MESG_TYPE_WARNING = "WRN";

        private readonly static bool FLAG_FINISH_YES = true;
        private readonly static bool FLAG_FINISH_NO = false;

        private readonly static string LOCATION_PROCESS_START = "Starting Process";
        private readonly static string LOCATION_PROCESS_END = "End Process";

        private readonly static string MESG_ID_PROCESS_START = "MSTD00000INF";
        private readonly static string MESG_ID_PROCESS_FINISH_SUCCESS = "MSTD00000INF";
        private readonly static string MESG_ID_PROCESS_FINISH_WARNING = "MSTD00000WRN";
        private readonly static string MESG_ID_PROCESS_FINISH_ERROR = "MSTD00000ERR";

        public readonly static string MESG_ID_COMMON_ERROR = "MSTD00000ERR";
        public readonly static string MESG_ID_COMMON_INFORMATION = "MSTD00000INF";

        public readonly static string MESG_KEY_FUNCTION_NAME = "[FunctionName]";

        public string UserId { get; set; }
        public long? ProcessId { get; set; }
        //public string ProcessName { get; set; }
        public int FunctionId { get; set; }
        public int ModuleId { get; set; }
        private IDBContext dbContext;
        public IDBContext DbContext { 
            get { return this.dbContext; }
            set 
            { 
                this.dbContext = value;
                if (this.dbContext != null)
                {
                    isHandleDbContext = false;
                }
            } 
        }

        private bool isHandleDbContext = true;
        //private DBContextExecutionMode lastDbContextExecutionMode;
        private int seqNo = 1;
        public string FunctionName { get; private set; }

        //public DefaultLogSession(string userId, string processId, string processName, string functionId, string moduleId)
        public LogDBSession(string userId, int functionId)
        {
            this.UserId = userId;
            this.FunctionId = functionId;

            ValidateInitData();
        }

        public LogDBSession(string userId, int moduleId, int functionId, IDBContext dbContext)
        {
            this.UserId = userId;
            this.ModuleId = moduleId;
            this.FunctionId = functionId;
            this.dbContext = dbContext;
            if (this.dbContext != null)
            {
                this.isHandleDbContext = false;
            }

            ValidateInitData();
        }

        private void ValidateInitData()
        {
            if (string.IsNullOrEmpty(UserId))
            {
                throw new Exception("User id cannot be empty/null");
            }

            //if (string.IsNullOrEmpty(FunctionId))
            if (FunctionId <= 0)
            {
                throw new Exception("Function id cannot be empty/null");
            }
        }

        public void WriteLogStart()
        {
            this.WriteLog(PROCESS_STS_IN_PROGRESS, FLAG_FINISH_NO, MESG_TYPE_INFORMATION,
                LOCATION_PROCESS_START, MESG_ID_PROCESS_START, 
                "Process [FunctionName] is started");
        }

        public void WriteLogFinishSuccess()
        {
            this.WriteLog(PROCESS_STS_SUCCESS, FLAG_FINISH_YES, MESG_TYPE_INFORMATION,
                LOCATION_PROCESS_END, MESG_ID_PROCESS_FINISH_SUCCESS, 
                string.Format("Process {0} is finish successfully", FunctionName));
        }

        public void WriteLogFinishWarning()
        {
            this.WriteLog(PROCESS_STS_SUCCESS, FLAG_FINISH_YES, MESG_TYPE_INFORMATION,
                LOCATION_PROCESS_END, MESG_ID_PROCESS_FINISH_WARNING,
                string.Format("Process {0} is finish with warning", FunctionName));
        }

        public void WriteLogFinishErrorBusiness()
        {
            this.WriteLog(PROCESS_STS_ERROR_BUSINESS, FLAG_FINISH_YES, MESG_TYPE_INFORMATION,
                LOCATION_PROCESS_END, MESG_ID_PROCESS_FINISH_ERROR,
                string.Format("Process {0} is finish with error business", FunctionName));
        }

        public void WriteLogFinishErrorSystem()
        {
            this.WriteLog(PROCESS_STS_ERROR_SYSTEM, FLAG_FINISH_YES, MESG_TYPE_INFORMATION,
                LOCATION_PROCESS_END, MESG_ID_PROCESS_FINISH_ERROR,
                string.Format("Process {0} is finish with error system", FunctionName));
        }

        public void WriteLogErrorBussiness(string logLocation, string logMesgId, string logMesg)
        {
            this.WriteLog(PROCESS_STS_ERROR_BUSINESS, FLAG_FINISH_NO, MESG_TYPE_ERROR,
                logLocation, logMesgId, logMesg);
        }

        public void WriteLogErrorSystem(string logLocation, string logMesgId, string logMesg)
        {
            this.WriteLog(PROCESS_STS_ERROR_SYSTEM, FLAG_FINISH_NO, MESG_TYPE_ERROR,
                logLocation, logMesgId, logMesg);
        }

        public void WriteLogInfo(string logLocation, string logMesgId, string logMesg)
        {
            this.WriteLog(PROCESS_STS_SUCCESS, FLAG_FINISH_NO, MESG_TYPE_INFORMATION,
                logLocation, logMesgId, logMesg);
        }

        public void WriteLogCommonErrorBussiness(string logLocation, string logMesg)
        {
            this.WriteLogErrorBussiness(logLocation, MESG_ID_COMMON_ERROR, logMesg);
        }

        public void WriteLogCommonErrorSystem(string logLocation, string logMesg)
        {
            this.WriteLogErrorSystem(logLocation, MESG_ID_COMMON_ERROR, logMesg);
        }

        public void WriteLogCommonInfo(string logLocation, string logMesg)
        {
            this.WriteLogInfo(logLocation, MESG_ID_COMMON_INFORMATION, logMesg);
        }

        private void WriteLog(string processSts, bool finishFlag, string logMesgType, 
            string logLocation, string logMesgId, string logMesg)
        {
            dynamic param = null;
            SqlParameter outputRetVal = CreateSqlParameterOutputReturnValue("RetVal");
            SqlParameter outputProcessId = CreateSqlParameterOutputProcessId("ProcessId", ProcessId);

            if (isHandleDbContext)
            {
                // create connection
                dbContext = DatabaseManager.Instance.GetContext();
                //dbContext.SetExecutionMode(DBContextExecutionMode.ByName);
            }
            else
            {
                //lastDbContextExecutionMode = dbContext.GetExecutionMode();
                //dbContext.SetExecutionMode(DBContextExecutionMode.ByName);
            }

            try
            {
                if (!ProcessId.HasValue)
                {
                    param = new
                    {
                        ModuleId = ModuleId,
                        FunctionId = FunctionId
                    };

                    FunctionName = dbContext.SingleOrDefault<string>("Commons/LogDatabase/GetFunctionName", param);
                }

                if (!string.IsNullOrEmpty(logMesg))
                {
                    logMesg = logMesg.Replace(MESG_KEY_FUNCTION_NAME, FunctionName);
                }

                param = new
                {
                    RetVal = outputRetVal,
                    LogMesg = logMesg,
                    UserId = UserId,
                    LogLocation = logLocation,
                    ProcessId = outputProcessId,
                    LogMesgId = logMesgId,
                    FunctionId = FunctionId,
                    ModuleId = ModuleId,
                    ProcessStatus = processSts,
                    FinishFlag = finishFlag                    
                };

                dbContext.Execute("Commons/LogDatabase/PutLog", param);

                // PutLog return value of pid
                if ((int)outputRetVal.Value <= 0)
                {
                    throw new LogDBException(string.Format("Failed Create Log, return value [{0}]", 
                        (int)outputRetVal.Value));
                }

                if (!ProcessId.HasValue)
                {
                    if (outputProcessId != null && outputProcessId.Value != null
                        && (long)outputProcessId.Value != 0 && !Convert.IsDBNull(outputProcessId.Value))
                    {
                        ProcessId = (long)outputProcessId.Value;
                    }
                }
            }
            finally
            {
                if (isHandleDbContext)
                {
                    dbContext.Close();
                }
                else
                {
                    //dbContext.SetExecutionMode(lastDbContextExecutionMode);
                }
            }
        }
    }
}
