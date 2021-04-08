using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PakAnas.Commons.Models;
using PakAnas.Commons.Repositories;

namespace PakAnas.Models
{
    public class DataLockRepo : BaseRepo
    {
        public long DATA_ID { get; set; }
        public string MODULE_ID { get; set; }
        public string FUNCTION_ID { get; set; }
        public string LOCKED_BY { get; set; }
        public string ACTION_TYPE { get; set; }

        #region Singleton

        public static DataLockRepo instance = null;

        public static DataLockRepo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataLockRepo();
                }
                return instance;
            }
        }

        #endregion Singleton

        public DataLockRepo()
        {
        }

        public DataLockRepo(long dataId, string moduleId, string functionId, string lockedBy, string actionType)
        {
            DATA_ID = dataId;
            MODULE_ID = moduleId;
            FUNCTION_ID = functionId;
            LOCKED_BY = lockedBy;
            ACTION_TYPE = actionType;
        }

        public RepoResult LockData()
        {
            SqlParameter outputRetVal = CreateSqlParameterOutputReturnValue("RetVal");
            SqlParameter outputErrMesg = CreateSqlParameterOutputErrMesg("ErrMesg");

            dynamic args = new
            {
                RetVal = outputRetVal,
                ErrMesg = outputErrMesg,
                DataId = DATA_ID,
                ModuleId = MODULE_ID,
                FunctionId = FUNCTION_ID,
                UserId = LOCKED_BY,
                ActionType = ACTION_TYPE
            };

            RepoResult repoResult = new RepoResult();
            try
            {
                Db.BeginTransaction();

                int result = Db.Execute("Shared/LockData", args);
                repoResult.Result = RepoResult.VALUE_SUCCESS;

                if ((int)outputRetVal.Value != 0)
                {
                    repoResult.Result = RepoResult.VALUE_ERROR;
                    string errMesg = string.Empty;
                    if (outputErrMesg != null && outputErrMesg.Value != null)
                    {
                        errMesg = outputErrMesg.Value.ToString();
                    }
                    repoResult.ErrMesgs = new string[1];
                    repoResult.ErrMesgs[0] = errMesg;
                    Db.AbortTransaction();
                }
                else
                {
                    Db.CommitTransaction();
                }
            }
            catch (Exception e)
            {
                repoResult.Result = RepoResult.VALUE_ERROR;
                repoResult.ErrMesgs = new string[1];
                repoResult.ErrMesgs[0] = e.Message;

                Db.AbortTransaction();
            }
            return repoResult;
        }

        public RepoResult ReleaseData()
        {
            SqlParameter outputRetVal = CreateSqlParameterOutputReturnValue("RetVal");
            SqlParameter outputErrMesg = CreateSqlParameterOutputErrMesg("ErrMesg");

            dynamic args = new
            {
                RetVal = outputRetVal,
                ErrMesg = outputErrMesg,
                DataId = DATA_ID,
                ModuleId = MODULE_ID,
                FunctionId = FUNCTION_ID,
                UserId = LOCKED_BY,
                ActionType = ACTION_TYPE
            };

            RepoResult repoResult = new RepoResult();
            try
            {
                Db.BeginTransaction();

                int result = Db.Execute("Shared/UnlockData", args);
                repoResult.Result = RepoResult.VALUE_SUCCESS;

                if ((int)outputRetVal.Value != 0)
                {
                    repoResult.Result = RepoResult.VALUE_ERROR;
                    string errMesg = string.Empty;
                    if (outputErrMesg != null && outputErrMesg.Value != null)
                    {
                        errMesg = outputErrMesg.Value.ToString();
                    }
                    repoResult.ErrMesgs = new string[1];
                    repoResult.ErrMesgs[0] = errMesg;
                    Db.AbortTransaction();
                }
                else
                {
                    Db.CommitTransaction();
                }
            }
            catch (Exception e)
            {
                repoResult.Result = RepoResult.VALUE_ERROR;
                repoResult.ErrMesgs = new string[1];
                repoResult.ErrMesgs[0] = e.Message;

                Db.AbortTransaction();
            }
            return repoResult;
        }
    }
}