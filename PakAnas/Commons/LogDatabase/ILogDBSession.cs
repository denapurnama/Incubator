using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toyota.Common.Database;

namespace PakAnas.Commons.LogDatabase
{
    public interface ILogDBSession
    {
        string UserId { get; set; }
        long? ProcessId { get; set; }
        int FunctionId { get; set; }
        int ModuleId { get; set; }
        IDBContext DbContext { get; set; }
        string FunctionName { get; }

        void WriteLogStart();
        void WriteLogFinishSuccess();
        //void WriteLogFinishWarning();
        void WriteLogFinishErrorBusiness();
        void WriteLogFinishErrorSystem();

        void WriteLogErrorBussiness(string logLocation, string logMesgId, string logMesg);
        void WriteLogErrorSystem(string logLocation, string logMesgId, string logMesg);
        void WriteLogInfo(string logLocation, string logMesgId, string logMesg);

        void WriteLogCommonErrorBussiness(string logLocation, string logMesg);
        void WriteLogCommonErrorSystem(string logLocation, string logMesg);
        void WriteLogCommonInfo(string logLocation, string logMesg);
    }
}
