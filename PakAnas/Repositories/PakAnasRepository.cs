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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PakAnas.Commons.Controllers;
using System.IO;
using PakAnas.Models;

using System.Text.RegularExpressions;

namespace PakAnas.Repositories
{
    public class PakAnasRepository :BaseRepo
    {

        private PakAnasRepository() { }

        private static readonly string SHEET_NAME_PAKANAS_MASTER = "PakAnas Master";
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

            int result = db.Execute("PakAnas/_InsertUpdate", args);

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

        #region GetByKey
        public PakAnass GetByKey(long? org_code)
        {

            dynamic args = new
            {
                org_Code = org_code
            };

            PakAnass result = Db.SingleOrDefault<PakAnass>("PakAnas/PakAnas_GetByKey", args);

            return result;
        }
        #endregion

        #region FindBySettingCd
        public string FindBySettingCd(string org_code)
        {
            dynamic args = new
            {
                SettingCode = org_code
            };
            string Result = Db.SingleOrDefault<string>
                ("PakAnas/PakAnas_FindBySettingCd", args);

            return Result;
        }
        #endregion

        #region DeleteMultiple
        public RepoResult DeleteMultiple(IDBContext db, IList<string> listKey)
        {
            SqlParameter outputRetVal = CreateSqlParameterOutputReturnValue("RetVal");
            SqlParameter outputErrMesg = CreateSqlParameterOutputErrMesg("ErrMesg");
            SqlParameter outputTblOfVarchar = CreateSqlParameterTblOfVarchar("TableOfVarchar",
                listKey, "dbo.TableOfVarchar");

            dynamic args = new
            {
                RetVal = outputRetVal,
                ErrMesg = outputErrMesg,
                TblOfVarchar = outputTblOfVarchar
            };

            int result = db.Execute("Pakanas/_DeleteMultiple", args);

            RepoResult repoResult = new RepoResult();
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
            }

            return repoResult;
        }
        #endregion

        #region DownloadDataToExcel
        public byte[] GenerateDownloadFile(IList<PakAnass> pakanasList)
        {
            byte[] result = null;

            try
            {
                result = CreateFile(pakanasList);
            }
            finally
            {

            }

            return result;
        }

        private byte[] CreateFile(IList<PakAnass> pakanasList)
        {
            Dictionary<string, string> headers = null;
            ISheet sheet1 = null;

            HSSFWorkbook workBook = null;
            byte[] result;
            int startRow = 0;

            workBook = new HSSFWorkbook();
            IDataFormat dataFormat = workBook.CreateDataFormat();
            short dateTimeFormat = dataFormat.GetFormat("dd/MM/yyy HH:mm:ss");

            ICellStyle cellStyleData = NPOIWriter.createCellStyleData(workBook);
            ICellStyle cellStyleHeader =
                NPOIWriter.createCellStyleColumnHeader(workBook);
            ICellStyle cellStyleDateTime =
                NPOIWriter.createCellStyleDataDate(workBook, dateTimeFormat);

            sheet1 = workBook.CreateSheet(
                NPOIWriter.EscapeSheetName(SHEET_NAME_PAKANAS_MASTER));
            sheet1.FitToPage = false;

            //Write Header manually
            headers = new Dictionary<string, string>();

            WriteDetail(workBook, sheet1, startRow,
                cellStyleHeader, cellStyleData, pakanasList);

            using (MemoryStream buffer = new MemoryStream())
            {
                workBook.Write(buffer);
                result = buffer.GetBuffer();
            }

            workBook = null;
            return result;
        }

        public void WriteDetail(HSSFWorkbook wb, ISheet sheet1, int startRow,
                                    ICellStyle cellStyleHeader, ICellStyle cellStyleData,
                                        IList<PakAnass> employeeList)
        {
            int rowIdx = startRow;
            int itemCount = 0;

            NPOIWriter.CreateSingleColHeader(wb, sheet1, 0, 0, cellStyleHeader, "org_code");
            NPOIWriter.CreateSingleColHeader(wb, sheet1, 0, 1, cellStyleHeader, "org_name");
            
            rowIdx = 1;
            foreach (PakAnass st in employeeList)
            {
                WriteDetailSingleData(wb, cellStyleData, st, sheet1, ++itemCount, rowIdx++,
                    cellStyleData);
            }
        }

        private void WriteDetailSingleData(HSSFWorkbook wb, ICellStyle cellStyle, PakAnass data,
                        ISheet sheet1, int rowCount, int rowIndex, ICellStyle cellStyleData)
        {
            IRow row = sheet1.CreateRow(rowIndex);
            int col = 0;

            NPOIWriter.createCellText(row, cellStyle, col++, data.org_code);
            NPOIWriter.createCellText(row, cellStyle, col++, data.org_name);
           

            sheet1.AutoSizeColumn(0);
        }
        #endregion

        #region UploadDelete Attachment
        public RepoResult InsertAttachment(string id,
            string tempFilePath, string tempFileName)
        {
            SqlParameter outputRetVal = CreateSqlParameterOutputReturnValue("RetVal");
            SqlParameter outputErrMesg = CreateSqlParameterOutputErrMesg("ErrMesg");

            dynamic args = new
            {
                RetVal = outputRetVal,
                ErrMesg = outputErrMesg,
                Id = id,
                TempFilePath = tempFilePath,
                tempFileName = tempFileName
            };

            Db.BeginTransaction();
            int result = Db.Execute("FileTemp/FileTemp_Insert", args);

            RepoResult repoResult = new RepoResult();
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

            return repoResult;
        }

        public RepoResult DeleteAttachment(string fileName)
        {
            fileName = Regex.Replace(fileName, @"\\\\Photo\\\\", "");

            SqlParameter outputRetVal = CreateSqlParameterOutputReturnValue("RetVal");
            SqlParameter outputErrMesg = CreateSqlParameterOutputErrMesg("ErrMesg");

            dynamic args = new
            {
                RetVal = outputRetVal,
                ErrMesg = outputErrMesg,
                TempFileName = fileName
            };
            
            Db.BeginTransaction();
            int result = Db.Execute("FileTemp/FileTemp_Delete", args);

            RepoResult repoResult = new RepoResult();
            repoResult.Result = RepoResult.VALUE_SUCCESS;

            if ((int)outputRetVal.Value !=0)
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

            return repoResult;
        }
            #endregion

    }
}