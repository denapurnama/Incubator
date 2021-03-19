using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PakAnas.Commons.Repositories;
using PakAnas.Models.PakAnas;
using Toyota.Common.Web.Platform;
using PakAnas.Commons.Models;
using Toyota.Common.Database;
using System.Data.SqlClient;

namespace PakAnas.Repositories
{
    public class PakAnasRepository :BaseRepo
    {
        #region Singleton
        private static PakAnasRepository instance = null;
        
        public static PakAnasRepository Instance
        {

            get
            {
                if (instance == null)
                {
                    instance = new PakAnasRepository();
                }
                return instance;
            }
        }
        #endregion

        #region Search
        public IList<PakAnass> Search(PakAnass pakanass, long rowStart, long rowEnd)
        {
            dynamic args = new
            {
                org_code = pakanass.org_code,
                org_name = pakanass.org_name,
                recordPerPage = rowStart,
                currentpage = rowEnd
            };
            IList<PakAnass> Result = Db.Fetch<PakAnass>("PakAnas/Pakanas_Search", args);

            return Result;
        }
        #endregion

        #region SearchCount
        public long SearchCount(PakAnass pak)
        {
            dynamic args = new
            {
                org_code = pak.org_code,
                org_name = pak.org_name
            };
            long Result = Db.SingleOrDefault<int>("PakAnas/Pakanas_SearchCount", args);

            return Result;
        }
        #endregion

        #region InsertOrUpdate
        public RepoResult InsertUpdate(IDBContext db, string userId,
            PakAnass data, string screenMode)
        {
            SqlParameter outputRetVal = CreateSqlParameterOutputReturnValue("RetVal");
            SqlParameter outputErrMesg = CreateSqlParameterOutputErrMesg("ErrMesg");

            dynamic args = new
            {
                RetVal = outputRetVal,
                ErrMesg = outputErrMesg,
                userId = userId,
                org_code = data.org_code,
                org_name = data.org_name,
                ScreenMode = screenMode
            };

            int result = db.Execute("PakAnas/InsetUpdate", args);

            RepoResult repoResult = new RepoResult();
            repoResult.Result = RepoResult.VALUE_SUCCESS;

            if ((int)outputRetVal.Value != 0)
            {
                repoResult.Result = RepoResult.VALUE_ERROR;
                string errMesg = string.Empty;
                if (outputErrMesg !=null && outputErrMesg.Value != null)
            
                {
                    errMesg = outputErrMesg.Value.ToString();
                }
                repoResult.ErrMesgs = new string[1];
                repoResult.ErrMesgs[0] = errMesg;
            }

            return repoResult;
        }
        #endregion
    }
}