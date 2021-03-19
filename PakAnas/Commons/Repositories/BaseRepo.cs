using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using PakAnas.Commons.Helpers;
using PakAnas.Commons.Models;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using System.Data.SqlClient;
using System.Data;

namespace PakAnas.Commons.Repositories
{
    public class BaseRepo
    {
        protected const string COLUMN_VAL = "VAL";
        protected const string TYPE_NAME_TABLE_OF_VARCHAR_2000 = "dbo.TableOfVarchar2000";
        protected const string TYPE_NAME_TABLE_OF_VARCHAR_10 = "dbo.TableOfVarchar10";
        protected const string ERR_MESG_SEPARATOR = @"\n";

        protected SqlParameter CreateSqlParameterOutputReturnValue(string parameterName) {
            var outputRetVal = new SqlParameter(parameterName, System.Data.SqlDbType.Int);
            outputRetVal.Direction = System.Data.ParameterDirection.Output;
            outputRetVal.Value = -1;
            //outputRetVal.SqlDbType = System.Data.SqlDbType.Int;

            return outputRetVal;
        }

        protected long CreateLog(string message, string messageId, string username, Exception ex)
        {
            string location = Util.SourceLocation(new System.Diagnostics.StackTrace(1));
            string What = message + " " + ex.Message;

            long r = Db.SingleOrDefault<long>("BaseRepo/CreateLog", new { what = What, user = username, where = location, id = messageId });
            return r;
        }
      
        protected SqlParameter CreateSqlParameterOutputErrMesg(string parameterName)
        {
            var outputErrMesg = new System.Data.SqlClient.SqlParameter(parameterName, System.Data.SqlDbType.VarChar, 2000);
            outputErrMesg.Direction = System.Data.ParameterDirection.Output;
            outputErrMesg.Value = string.Empty;

            return outputErrMesg;
        }

        protected SqlParameter CreateSqlParameterOutputProcessId(string parameterName, long? val = null)
        {
            var outputProcessId = new System.Data.SqlClient.SqlParameter(parameterName, System.Data.SqlDbType.BigInt);
            if (val.HasValue)
            {
                outputProcessId.Direction = System.Data.ParameterDirection.InputOutput;
                outputProcessId.Value = val.Value;
            }
            else
            {
                outputProcessId.Direction = System.Data.ParameterDirection.Output;
                outputProcessId.Value = DBNull.Value;
            }
            
            return outputProcessId;
        }

        protected SqlParameter CreateSqlParameterOutputVarchar(string parameterName, int length)
        {
            var outputVarchar = new System.Data.SqlClient.SqlParameter(parameterName, System.Data.SqlDbType.VarChar, length);
            outputVarchar.Direction = System.Data.ParameterDirection.Output;
            outputVarchar.Value = string.Empty;

            return outputVarchar;
        }

        protected SqlParameter CreateSqlParameterOutputBigInt(string parameterName)
        {
            var outputBigInt = new System.Data.SqlClient.SqlParameter(parameterName, System.Data.SqlDbType.BigInt);
            outputBigInt.Direction = System.Data.ParameterDirection.Output;
            outputBigInt.Value = DBNull.Value;

            return outputBigInt;
        }

        protected SqlParameter CreateSqlParameterOutputBit(string parameterName)
        {
            var outputBit = new SqlParameter(parameterName, System.Data.SqlDbType.Bit);
            outputBit.Direction = System.Data.ParameterDirection.Output;
            outputBit.Value = DBNull.Value;

            return outputBit;
        }

        protected SqlParameter CreateSqlParameterTblOfVarchar10(string parameterName, IList<string> listData, string typeName)
        {
            DataTable table = new DataTable();

            table.Columns.Add(COLUMN_VAL, type: typeof(string));

            if (listData != null)
            {
                foreach (string data in listData)
                {
                    DataRow row = table.NewRow();
                    row[COLUMN_VAL] = data;
                    table.Rows.Add(row);
                }
            }

            var paramStruct = new System.Data.SqlClient.SqlParameter(parameterName, System.Data.SqlDbType.Structured);
            paramStruct.SqlDbType = SqlDbType.Structured; // According to marc_s
            paramStruct.SqlValue = table;
            //paramArr.ParameterName = "@ri_t_supply_dtl_id";
            //paramStruct.TypeName = "dbo.TableOfVarchar10";
            paramStruct.TypeName = typeName;

            return paramStruct;
        }

        protected SqlParameter CreateSqlParameterTblOfVarchar(string parameterName, IList<string> listData, string typeName)
        {
            DataTable table = new DataTable();

            table.Columns.Add(COLUMN_VAL, type: typeof(string));

            if (listData != null)
            {
                foreach (string data in listData)
                {
                    DataRow row = table.NewRow();
                    row[COLUMN_VAL] = data;
                    table.Rows.Add(row);
                }
            }

            var paramStruct = new System.Data.SqlClient.SqlParameter(parameterName, System.Data.SqlDbType.Structured);
            paramStruct.SqlDbType = SqlDbType.Structured; // According to marc_s
            paramStruct.SqlValue = table;
            //paramArr.ParameterName = "@ri_t_supply_dtl_id";
            //paramStruct.TypeName = "dbo.TableOfVarchar10";
            paramStruct.TypeName = typeName;

            return paramStruct;
        }

        protected string[] GetMesgFromOutputErrMesg(SqlParameter outputErrMesg)
        {
            if (outputErrMesg == null || Convert.IsDBNull(outputErrMesg) || Convert.IsDBNull(outputErrMesg.Value))
            {
                return null;
            }

            string[] result = null;
            string allMesg = (string)outputErrMesg.Value;

            if (!string.IsNullOrEmpty(allMesg))
            {
                //result = Regex.Split(allMesg, ERR_MESG_SEPARATOR);
                result = allMesg.Split(new string[] {ERR_MESG_SEPARATOR}, StringSplitOptions.RemoveEmptyEntries);
            }

            return result;
        }

        protected RepoResult GenerateRepoResult(SqlParameter outputRetVal, SqlParameter outputErrMesg)
        {
            return this.GenerateRepoResult(outputRetVal, outputErrMesg, null);
        }

        protected RepoResult GenerateRepoResult(SqlParameter outputRetVal, SqlParameter outputErrMesg, SqlParameter outputProcessId)
        {
            RepoResult repoResult = new RepoResult();

            if ((int)outputRetVal.Value != 0)
            {
                repoResult.Result = RepoResult.VALUE_ERROR;
                repoResult.ErrMesgs = GetMesgFromOutputErrMesg(outputErrMesg);
            }
            else
            {
                repoResult.Result = RepoResult.VALUE_SUCCESS;
                repoResult.SuccMesgs = GetMesgFromOutputErrMesg(outputErrMesg);
            }

            if (outputProcessId != null && !Convert.IsDBNull(outputProcessId)
                && !Convert.IsDBNull(outputProcessId.Value))
            {
                repoResult.ProcessId = Convert.ToString(outputProcessId.Value);
            }

            return repoResult;
        }

        public DateTime GetDbCurrDt(IDBContext db)
        {
            return db.SingleOrDefault<DateTime>("select getdate();");
        }

        public static readonly string PROP_DBCONTEXT = "BaseRepo.DbContext";
        public static IDictionary CurrentItems
        {
            get
            {
                if (HttpContext.Current != null)
                    return HttpContext.Current.Items;
                else
                    return new Dictionary<string, object>();
            }
        }

        public static IDBContext Db
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    IDBContext db = null;

                    if (HttpContext.Current.Items.Contains(PROP_DBCONTEXT))
                    {
                        db = HttpContext.Current.Items[PROP_DBCONTEXT] as IDBContext;

                        Util.TraceCall("db", " @ {0}");
                    }

                    if (db == null)
                    {
                        db = DatabaseManager.Instance.GetContext();
                        HttpContext.Current.Items.Add(PROP_DBCONTEXT, db);

                        Util.TraceCall("db", "Open @ {0}");
                    }


                    return db;
                }
                else
                    return null;

            }
        }

        public static void CloseDb()
        {
            IDBContext db = null;
            bool hasDb = false;
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items.Contains(PROP_DBCONTEXT))
                {
                    db = HttpContext.Current.Items[PROP_DBCONTEXT] as IDBContext;
                    hasDb = (db != null);
                }
            }

            if (hasDb)
            {
                Util.TraceCall("db", "Close @ {0}");
                db.Close();
                HttpContext.Current.Items.Remove(PROP_DBCONTEXT);
            }
            else
            {
                Util.TraceCall("db", "NotClosed@{0}");
            }
        }
    }
}
