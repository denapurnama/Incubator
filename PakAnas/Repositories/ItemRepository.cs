using System.Collections.Generic;
using PakAnas.Commons.Repositories;
using Toyota.Common.Database;
using PakAnas.Commons.Models;
using System.Data.SqlClient;
using Adaptive.Models;
using PakAnas.Models.Items;

using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using PakAnas.Commons.Controllers;
using System.IO;

using System.Data;
using System.Text.RegularExpressions;

namespace Item.Repositories
{
    public class ItemRepository : BaseRepo
    {

        private ItemRepository() { }

        private static readonly string SHEET_NAME_EMPLOYEE_MASTER = "Item Master";

        #region Singleton
        private static ItemRepository instance = null;
        public static ItemRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemRepository();
                }
                return instance;
            }
        }
        #endregion

        #region Search
        public IList<Items> Search(Items item, long rowStart, long rowEnd)
        {
            dynamic args = new
            {
                ItemCode = item.ITEM_CODE,
                ItemName = item.ITEM_NAME,
                ItemUm = item.ITEM_UM,
                RowStart = rowStart,
                RowEnd = rowEnd
            };
            IList<Items> Result = Db.Fetch<Items>("Item/Item_Search", args);

            return Result;
        }
        #endregion


        /*
        #region GetCmbListByType
        public IList<CmbAPPSetting> CmbListByType(string typeCode)
        {
            dynamic args = new
            {
                TypeCode = typeCode
            };
            IList<CmbAPPSetting> Result = Db.Fetch<CmbAPPSetting>
                ("Employee/Employee_CmbListByType", args);

            return Result;
        }
        #endregion
         */        
        #region FindBySettingCd
        public string FindBySettingCd(string settingCode)
        {
            dynamic args = new
            {
                SettingCode = settingCode
            };
            string Result = Db.SingleOrDefault<string>
                ("Item/Item_FindBySettingCd", args);

            return Result;
        }
        #endregion
      /*
        #region FindBySystemValTxtWithSysType
        public string FindBySystemValTxtWithSysType(string settingName, string typeCode)
        {
            dynamic args = new
            {
                SettingName = settingName,
                TypeCode = typeCode
            };
            string Result = Db.SingleOrDefault<string>
                ("Employee/Employee_FindBySystemValTxtWithSysType", args);

            return Result;
        }
        #endregion    
                 */
        #region SearchCount
        public long SearchCount(Items item)
        {
            dynamic args = new
            {
                ItemCode = item.ITEM_CODE,
                ItemName = item.ITEM_NAME,
                ItemUm = item.ITEM_UM
            };
            long Result = Db.SingleOrDefault<long>("Item/Item_SearchCount", args);

            return Result;
        }
        #endregion

        #region InsertOrUpdate
        public RepoResult InsertUpdate(IDBContext db, string userId,
            Items data, string screenMode)
        {
            SqlParameter outputRetVal = CreateSqlParameterOutputReturnValue("RetVal");
            SqlParameter outputErrMesg = CreateSqlParameterOutputErrMesg("ErrMesg");

            dynamic args = new
            {
                RetVal = outputRetVal,
                ErrMesg = outputErrMesg,
                UserId = userId,
                ItemCode = data.ITEM_CODE,
                ItemName = data.ITEM_NAME,
                ItemUm = data.ITEM_UM,
                ScreenMode = screenMode
            };

            int result = db.Execute("Item/Item_InsertUpdate", args);

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

            int result = db.Execute("Item/Item_DeleteMultiple", args);

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

        #region GetByKey
        public Items GetByKey(string itemscode)
        {
            dynamic args = new
            {
                ItemId = itemscode
            };

            Items result = Db.SingleOrDefault<Items>("Item/Item_GetByKey", args);

            return result;
        }
        #endregion

        #region DownloadDataToExcel
        public byte[] GenerateDownloadFile(IList<Items> ItemList)
        {
            byte[] result = null;

            try
            {
                result = CreateFile(ItemList);
            }
            finally
            {

            }

            return result;
        }

        private byte[] CreateFile(IList<Items> ItemList)
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
                NPOIWriter.EscapeSheetName(SHEET_NAME_EMPLOYEE_MASTER));
            sheet1.FitToPage = false;

            //Write Header manually
            headers = new Dictionary<string, string>();

            WriteDetail(workBook, sheet1, startRow,
                cellStyleHeader, cellStyleData, ItemList);

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
                                        IList<Items> ItemList)
        {
            int rowIdx = startRow;
            int itemCount = 0;

            NPOIWriter.CreateSingleColHeader(wb, sheet1, 0, 0, cellStyleHeader, "Item Code");
            NPOIWriter.CreateSingleColHeader(wb, sheet1, 0, 1, cellStyleHeader, "Item Name");
            NPOIWriter.CreateSingleColHeader(wb, sheet1, 0, 2, cellStyleHeader, "Item Um");

            rowIdx = 1;
            foreach (Items st in ItemList)
            {
                WriteDetailSingleData(wb, cellStyleData, st, sheet1, ++itemCount, rowIdx++,
                    cellStyleData);
            }
        }

        private void WriteDetailSingleData(HSSFWorkbook wb, ICellStyle cellStyle, Items data,
                        ISheet sheet1, int rowCount, int rowIndex, ICellStyle cellStyleData)
        {
            IRow row = sheet1.CreateRow(rowIndex);
            int col = 0;

            NPOIWriter.createCellText(row, cellStyle, col++, data.ITEM_CODE);
            NPOIWriter.createCellText(row, cellStyle, col++, data.ITEM_NAME);
            NPOIWriter.createCellText(row, cellStyle, col++, data.ITEM_UM);

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
                TempFileName = tempFileName
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
        #endregion

    }
}