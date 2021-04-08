using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PakAnas.Commons.Constants;
using PakAnas.Commons.Controllers;
using PakAnas.Commons.Models;
using PakAnas.Models;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using PakAnas.Repositories;

using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

using System.IO;

using System.Diagnostics;
using PakAnas.Commons.Helpers;

namespace PakAnas.Controllers
{
    public class PoController : HelperController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private PoRepo approvalRepo = PoRepo.Instance;

        public const string DOWNLOAD_TEMPLATE_FILE_NAME = "DownloadTemplateTabelItem.xls";
        public const int DATA_ROW_INDEX_START = 3;

        protected override void Startup()
        {
            Settings.Title = "Tabel PO";

            int currentPage = 1;
            int recordPerPage = PagingModel.DEFAULT_RECORD_PER_PAGE;
            Approval data = new Approval();

            DoSearch(data, currentPage, recordPerPage);

            GetFunction();
            FindByType();
            GetCmboxPersonList(string.Empty);
            GetCmboxOrgList(string.Empty);
        }

         #region GetTypeSizeUpload
        private void getTypeSizeUpload()
        {
            string UploadType =
                approvalRepo.FindBySettingCd(CommonConstant.SUFE_EMPLOYEE_INPUT);
            ViewData["UploadType"] = UploadType;

            string UploadSize =
                approvalRepo.FindBySettingCd(CommonConstant.SUFS_EMPLOYEE_INPUT);
            ViewData["UploadSize"] = Double.Parse(UploadSize);

        }
         #endregion

        public ActionResult Search(Approval Po, int currentPage, int recordPerPage)
        {
            try
            {
                DoSearch(Po, currentPage, recordPerPage);
            }
            catch (Exception ex)
            {
                return Json("Error : " + ex.Message, JsonRequestBehavior.AllowGet);
            }
            return PartialView("_GridViews");
        }
          

        public void DoSearch(Approval data, int currentPage, int recordPerPage)
        {
            PagingModel pmodel = new PagingModel(approvalRepo.SearchCount(data), currentPage, recordPerPage);
            ViewData["Paging"] = pmodel;

            IList<Approval> listApproval = approvalRepo.Search(data, pmodel.Start, pmodel.End);
            ViewData["ListData"] = listApproval;
        }

        public AjaxResult Validation(Approval data)
        {
            AjaxResult ajaxResult = new AjaxResult();

            ajaxResult.ErrMesgs = new string[1];
            ajaxResult.Result = AjaxResult.VALUE_ERROR;

            if (data.po_no == null || data.po_no == "")
            {
                ajaxResult.ErrMesgs[0] = "ERROR CTRL: Purchase Order should not be empty";
            }
            else if (data.pr_no == null || data.pr_no == "")
            {
                ajaxResult.ErrMesgs[0] = "ERROR CTRL: Purchase Repeat should not be empty";
            }
            else if (data.po_date == null)
            {
                ajaxResult.ErrMesgs[0] = "ERROR CTRL: Purchase Date should not be empty";
            }
            else if (data.supplier_code == null || data.supplier_code == "")
            {
                ajaxResult.ErrMesgs[0] = "ERROR CTRL: Supplier Code should not be empty";
            }
            else if (data.listApprovalFlow == null || data.listApprovalFlow.Count == 0)
            {
                ajaxResult.ErrMesgs[0] = "ERROR CTRL: Please choose at least on data to be Approval Detail List";
            }
            else if (data.listApprovalFlow.Count > 0)
            {
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;

                foreach (ApprovalFlow dataDtl in data.listApprovalFlow)
                {
                    if (dataDtl.item_code == null || dataDtl.item_code == "")
                    {
                        ajaxResult.ErrMesgs[0] = "ERROR CTRL: Item Code should not be empty";
                        ajaxResult.Result = AjaxResult.VALUE_ERROR;
                        break;
                    }
                    if (dataDtl.request_qty == null || dataDtl.request_qty == "")
                    {
                        ajaxResult.ErrMesgs[0] = "ERROR CTRL: Request Qty should not be empty";
                        ajaxResult.Result = AjaxResult.VALUE_ERROR;
                        break;
                    }
                    if (dataDtl.unit_price == null || dataDtl.unit_price == "")
                    {
                        ajaxResult.ErrMesgs[0] = "ERROR CTRL: Unit Price should not be empty";
                        ajaxResult.Result = AjaxResult.VALUE_ERROR;
                        break;
                    }
                }
            }
            else
            {
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
            }

            return ajaxResult;
        }

        public JsonResult AddEditSave(string screenMode, PakAnas.Models.Approval data)
        {
            AjaxResult ajaxResult = new AjaxResult();
            RepoResult repoResult = null;

            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();

                ajaxResult = Validation(data); //Validasi Controller

                if (AjaxResult.VALUE_SUCCESS.Equals(ajaxResult.Result))
                {

                    if (CommonConstant.SCREEN_MODE_ADD.Equals(screenMode))
                    {
                        repoResult = approvalRepo.Insert(GetLoginUserId(), data);
                    }
                    else if (CommonConstant.SCREEN_MODE_EDIT.Equals(screenMode))
                    {
                        repoResult = approvalRepo.Update(GetLoginUserId(), data);
                    }

                    CopyPropertiesRepoToAjaxResult(repoResult, ajaxResult);

                    if (AjaxResult.VALUE_ERROR.Equals(ajaxResult.Result))
                    {
                        db.AbortTransaction();
                    }
                    else
                    {
                        db.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] { 
                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                };
            }
            finally
            {
                db.Close();
            }

            return Json(ajaxResult);
        }

        // Function Code, Name
        public void GetFunction()
        {
            IDBContext db = databaseManager.GetContext();
            ViewData["ListApproval"] = approvalRepo.GetFunctionById(db, "Function");
        }

        // Approver Type
        public void FindByType()
        {
            IDBContext db = databaseManager.GetContext();
            ViewData["ListType"] =
                approvalRepo.FindByType(CommonConstant.SYSTEM_TYPE_SPTT_APPROVER_TYPE);
        }

        // Person
        public void GetCmboxPersonList(string empNoreg)
        {
            IDBContext db = databaseManager.GetContext();
            ViewData["ListPerson"] = approvalRepo.GetAllPersonByCriteria(empNoreg);
        }

        // Organization
        public void GetCmboxOrgList(string orgTitle)
        {
            IDBContext db = databaseManager.GetContext();
            ViewData["ListOrg"] = approvalRepo.GetAllOrgByCriteria(orgTitle);
        }

        public JsonResult GetBuyerList()
        {
            AjaxResult ajaxResult = new AjaxResult();
            IList<Buyer> result = null;
            IDBContext db = databaseManager.GetContext();

            try
            {
                result = approvalRepo.GetBuyerList();

                if (result == null)
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] { 
                        string.Format("No Data with the selected key found, please refresh the screen first")
                    };

                    return Json(ajaxResult);
                }

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                ajaxResult.Params = new object[] {
                    result
                };
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] { 
                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                };
            }
            finally
            {
                db.Close();
            }

            return Json(ajaxResult);
        }

        public JsonResult GetByKey(long approvalId)
        {
            AjaxResult ajaxResult = new AjaxResult();
            PakAnas.Models.Approval result = null;

            try
            {
                result = approvalRepo.GetByKeyWithDtl(approvalId);

                if (result == null)
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] { 
                        string.Format("No Data with the selected key found,"+
                        "please refresh the screen first")
                    };

                    return Json(ajaxResult);
                }

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                ajaxResult.Params = new object[] {
                    result
                };
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] { 
                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                };
            }

            return Json(ajaxResult);
        }

        public JsonResult DeleteMultiple(List<string> selectedKeys)
        {
            AjaxResult ajaxResult = new AjaxResult();
            RepoResult repoResult = null;

            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();

                repoResult = approvalRepo.DeleteMultiple(db, selectedKeys);

                CopyPropertiesRepoToAjaxResult(repoResult, ajaxResult);

                if (AjaxResult.VALUE_ERROR.Equals(ajaxResult.Result))
                {
                    db.AbortTransaction();
                }
                else
                {
                    db.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] { 
                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                };
            }
            finally
            {
                db.Close();
            }

            return Json(ajaxResult);
        }

        public JsonResult CheckDataLockingStatus(long dataId, string moduleId, string functionId)
        {
            AjaxResult ajaxResult = new AjaxResult();
            RepoResult repoResult = null;

            try
            {
                repoResult = this.LockData(dataId, moduleId, functionId, GetLoginUserId(), CommonConstant.DATA_LOCK_ACTION_TYPE.SDLAT_EDIT);
                CopyPropertiesRepoToAjaxResult(repoResult, ajaxResult);
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                };
            }

            return Json(ajaxResult);
        }

        public JsonResult UnlockData(long dataId, string moduleId, string functionId)
        {
            AjaxResult ajaxResult = new AjaxResult();
            RepoResult repoResult = null;

            try
            {
                repoResult = this.UnlockData(dataId, moduleId, functionId, GetLoginUserId(), CommonConstant.DATA_LOCK_ACTION_TYPE.SDLAT_EDIT);
                CopyPropertiesRepoToAjaxResult(repoResult, ajaxResult);
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                };
            }

            return Json(ajaxResult);
        }
        #region DownloadDataToExcel
        public void DownloadFileExcel(Approval Po, int? PageFlag)
        {
            byte[] result = null;
            string fileName = null;

            try
            {
                if (PageFlag == 0)
                {
                    Po.RowsPerPage = Int32.MaxValue;
                }

                IList<Approval> data =
                    approvalRepo.Search(Po, Po.CurrentPage,
                        Po.RowsPerPage);

                fileName = string.Format("Purchase Order-{0}.xls",
                        DateTime.Now.ToString("yyyyMMddHHmmss"));
                result = approvalRepo.GenerateDownloadFile(data);
            }
            catch (Exception e)
            {
                //
            }

            this.SendDataAsAttachment(fileName, result);
        }

        /*
       public IList<Items> SearchCommon(Items o, ViewDataDictionary v, string pageModelViewData, string listViewData)
       {
           Type t = o.GetType();

           int pDot = t.Namespace.LastIndexOf(".");
           int pLen = t.Namespace.Length;

           string dir = (pDot > 1 && (pDot < (pLen - 1))) ? (t.Namespace.Substring(pDot + 1, pLen - pDot - 1) + "/") : "";

           string qCount = dir + t.Name + "_Count";
           string qList = dir + t.Name + "_List";

           IDBContext db = BaseRepo.Db;

           long count = db.SingleOrDefault<int>(qCount, o);

           BaseModel m = o as BaseModel;
           PagingModel pmodel = null;
           if (m.RowsPerPage <= 0)
           {
               pmodel = new PagingModel(count, 1, PagingModel.DEFAULT_RECORD_PER_PAGE);
           }
           else
           {
               pmodel = new PagingModel(count, m.CurrentPage, m.RowsPerPage);
           }

           v[pageModelViewData] = pmodel;

           IList<Items> l = db.Fetch<Items>(qList, o);

           return l;
       }
            */
        #endregion

        #region DownloadTemplate
        public void DownloadTemplate()
        {
            string fileName = "DownloadTemplatePurchaseToUpload.xls";
            string filesTMp = HttpContext.Request.MapPath("~" +
                CommonConstant.TEMPLATE_EXCEL_DIR + "/" + fileName);
            FileStream ftmp = new FileStream(filesTMp, FileMode.Open, FileAccess.Read);
            byte[] result = null;

            HSSFWorkbook workbook = new HSSFWorkbook(ftmp);

            ICellStyle cellStyleData = NPOIWriter.createCellStyleData(workbook);
            //ICellStyle cellStyleHeader = NPOIWriter.createCellStyleColumnHeader(workbook);

            HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(0);

            int row = DATA_ROW_INDEX_START;
            IRow Hrow;

            for (int i = 1; i <= 100; i++)
            {
                Hrow = sheet.CreateRow(row);

                for (int x = 0; x <= 2; x++)
                {
                    Hrow.CreateCell(x);
                    Hrow.GetCell(x).CellStyle = cellStyleData;
                }

                row++;
            }

            int rowMin = row - 1;

            ftmp.Close();
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                result = ms.GetBuffer();
            }

            this.SendDataAsAttachment(fileName, result);
        }
        #endregion
    }
}
