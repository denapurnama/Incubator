using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PakAnas.Commons.Models;
using PakAnas.Commons.Repositories;
using PakAnas.Models;
using Toyota.Common.Database;

using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using PakAnas.Commons.Controllers;
using System.IO;

using System.Text.RegularExpressions;

namespace PakAnas.Repositories
{
    public class PoRepo : BaseRepo
    {
        private PoRepo() { }

        private static readonly string SHEET_NAME_EMPLOYEE_MASTER = "Po Master";

        #region Singleton
        public static PoRepo instance = null;
        public static PoRepo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PoRepo();
                }
                return instance;
            }
        }
        #endregion


        #region FindBySettingCd
        public string FindBySettingCd(string settingCode)
        {
            dynamic args = new
            {
                SettingCode = settingCode
            };
            string Result = Db.SingleOrDefault<string>
                ("Po/Po_FindBySettingCd", args);

            return Result;
        }
        #endregion

        // method repository untuk pencarian data grid view
        public IList<Approval> Search(Approval Po, long rowStart, long rowEnd)
        {
            // dynamic args berfungsi sebagai parameter berserta nilai yang akan di assist ke dalam query (Approval_Search.sql)
            // **jadi harus tersedia/sesuai penulisan nya dengan yang ada di dalam query tsb. sbg contoh: DateFrom, ApprovName dll
            dynamic args = new
            {
                Po = Po.po_no,
                Pr = Po.pr_no,
                PoDate = Po.po_date,
                Supplier = Po.supplier_code,
                RowStart = rowStart,
                RowEnd = rowEnd
            };

            // IList<Approval> sebagai media tampungan data dari hasil execute query Approval_Search.sql
            // **jika select column nama, column harus sudah ada di dalam model Approval
            // **Db.Fecth berfungsi untuk execute query dengan balikan 1 baris record atau lebih
            IList<Approval> result = Db.Fetch<Approval>("Po/Po_Search", args);
            return result;
        }

        // method repository untuk mendapatkan jumlah data yang akan di tampilkan (paging pada grid view)
        public long SearchCount(Approval Po)
        {
            dynamic args = new
            {
                Po = Po.po_no,
                Pr = Po.pr_no,
                PoDate = Po.po_date,
                Supplier = Po.supplier_code
            };
            // Db.SingleOrDefault berfungsi untuk execute query dengan balikan 1 baris record atau tidak sama sekali / tipe scalar
            long result = Db.SingleOrDefault<int>("Po/Po_SearchCount", args);
            return result;
            // return result disini berfungsi mengembalikan nilai kepada method pemanggil (dalam case method dalam controller)
        }

        public IList<Buyer> GetAllPersonByCriteria(string empNoreg)
        {
            dynamic args = new
            {
                NOREG = empNoreg
            };
            return Db.Fetch<Buyer>("Po/Po_GetAllPersonByCriteria", args);
        }

        public IList<OrgHierarchy> GetAllOrgByCriteria(string orgTitle)
        {
            dynamic args = new
            {
                ORG_TITLE = orgTitle
            };
            return Db.Fetch<OrgHierarchy>("Po/Po_GetAllOrgByCriteria", args);
        }

        public IList<ApprovalSystem> FindByType(string systemType)
        {
            dynamic args = new
            {
                SystemType = systemType
            };
            IList<ApprovalSystem> Result =
                Db.Fetch<ApprovalSystem>("Po/Po_FindByType", args);

            return Result;
        }

        public IList<Approval> GetFunctionById(IDBContext db, string functionName)
        {
            dynamic args = new
            {
                FunctionName = functionName
            };

            IList<Approval> Result =
                db.Fetch<Approval>("Po/Po_GetFunctionById", args);
            return Result;
        }


        public RepoResult Insert(string userId, Approval app)
        {
            SqlParameter outputRetVal = CreateSqlParameterOutputReturnValue("RetVal");
            SqlParameter outputErrMesg = CreateSqlParameterOutputErrMesg("ErrMesg");
            SqlParameter outputTblApprovalFlow =
                CreateSqlParameterOutputTblOfApprovalFlow("TableOfVarPo",
                    app.listApprovalFlow, "dbo.TableOfVarPo");

            dynamic args = new
            {
                RetVal = outputRetVal,
                ErrMesg = outputErrMesg,
                UserId = userId,
                Po = app.po_no,
                Pr = app.pr_no,
                PoDate = app.po_date,
                Supplier = app.supplier_code,
                TableApprovalFlow = outputTblApprovalFlow
            };

            Db.BeginTransaction();
            int result = Db.Execute("Po/Po_Insert", args);

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

        protected SqlParameter CreateSqlParameterOutputTblOfApprovalFlow(string parameterName,
            IList<ApprovalFlow> listData, string typeName)
        {
            DataTable table = new DataTable();

            table.Columns.Add("item_code", type: typeof(string));
            table.Columns.Add("request_qty", type: typeof(int));
            table.Columns.Add("unit_price", type: typeof(int));

            if (listData != null)
            {
                foreach (ApprovalFlow data in listData)
                {
                    DataRow row = table.NewRow();
                    row["item_code"] = data.item_code;
                    row["request_qty"] = data.request_qty;
                    row["unit_price"] = data.unit_price;

                    table.Rows.Add(row);
                }
            }

            var paramStruct = new System.Data.SqlClient.SqlParameter(parameterName,
                System.Data.SqlDbType.Structured);
            paramStruct.SqlDbType = SqlDbType.Structured; // According to marc_s
            paramStruct.SqlValue = table;
            paramStruct.TypeName = typeName;

            return paramStruct;
        }

        public RepoResult Update(string userId, Approval app)
        {
            SqlParameter outputRetVal = CreateSqlParameterOutputReturnValue("RetVal");
            SqlParameter outputErrMesg = CreateSqlParameterOutputErrMesg("ErrMesg");
            SqlParameter outputTblApprovalFlow = CreateSqlParameterOutputTblOfApprovalFlow("TableOfVarPo", app.listApprovalFlow, "dbo.TableOfVarPo");

            dynamic args = new
            {
                RetVal = outputRetVal,
                ErrMesg = outputErrMesg,
                UserId = userId,
                ApprovalId = app.pr_no,
                DateFrom = app.create_on,
                DateTo = app.VALID_TO_DT,
                ApprovName = app.po_date,
                FunctionId = app.supplier_code,
                TableApprovalFlow = outputTblApprovalFlow
            };

            Db.BeginTransaction();
            int result = Db.Execute("Po/Po_Update", args);

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

        public IList<Buyer> GetBuyerList()
        {
            dynamic args = new
            {

            };
            IList<Buyer> Result = Db.Fetch<Buyer>("SPBuyer/SPBuyer_FindBuyerPByCriteria", args);

            return Result;
        }

        public Approval GetByKeyWithDtl(long approvalId)
        {
            dynamic args = new
            {
                ApprovalId = approvalId
            };

            Approval result =
                Db.SingleOrDefault<Approval>("Po/Po_GetByKey", args);
            result.listApprovalFlow =
                Db.Fetch<ApprovalFlow>("Po/Po_GetListByApprovalId", args);

            return result;
        }

        public RepoResult DeleteMultiple(IDBContext db, IList<string> listKey)
        {
            SqlParameter outputRetVal = CreateSqlParameterOutputReturnValue("RetVal");
            SqlParameter outputErrMesg = CreateSqlParameterOutputErrMesg("ErrMesg");
            SqlParameter outputTblApprovalId = CreateSqlParameterTblOfVarchar("TableOfVarchar", listKey, "dbo.TableOfVarchar");

            dynamic args = new
            {
                RetVal = outputRetVal,
                ErrMesg = outputErrMesg,
                TableOfVarchar = outputTblApprovalId
            };

            int result = db.Execute("Po/Po_DeleteMultiple", args);

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

        #region DownloadDataToExcel
        public byte[] GenerateDownloadFile(IList<Approval> PoList)
        {
            byte[] result = null;

            try
            {
                result = CreateFile(PoList);
            }
            finally
            {

            }

            return result;
        }

        private byte[] CreateFile(IList<Approval> PoList)
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
                cellStyleHeader, cellStyleData, PoList);

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
                                        IList<Approval> PoList)
        {
            int rowIdx = startRow;
            int itemCount = 0;

            NPOIWriter.CreateSingleColHeader(wb, sheet1, 0, 0, cellStyleHeader, "Purchase Order No");
            NPOIWriter.CreateSingleColHeader(wb, sheet1, 0, 1, cellStyleHeader, "Purchase Repeat No");
            NPOIWriter.CreateSingleColHeader(wb, sheet1, 0, 2, cellStyleHeader, "Purchase Order Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet1, 0, 3, cellStyleHeader, "Supplier Code");

            rowIdx = 1;
            foreach (Approval st in PoList)
            {
                WriteDetailSingleData(wb, cellStyleData, st, sheet1, ++itemCount, rowIdx++,
                    cellStyleData);
            }
        }

        private void WriteDetailSingleData(HSSFWorkbook wb, ICellStyle cellStyle, Approval data,
                        ISheet sheet1, int rowCount, int rowIndex, ICellStyle cellStyleData)
        {
            IRow row = sheet1.CreateRow(rowIndex);
            int col = 0;

            NPOIWriter.createCellText(row, cellStyle, col++, data.po_no);
            NPOIWriter.createCellText(row, cellStyle, col++, data.pr_no);
            NPOIWriter.createCellText(row, cellStyle, col++, data.PO_DATE_STR);
            NPOIWriter.createCellText(row, cellStyle, col++, data.supplier_code);

            sheet1.AutoSizeColumn(0);
        }
        #endregion

    }
}