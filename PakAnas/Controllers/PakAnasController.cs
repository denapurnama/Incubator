using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PakAnas.Commons.Controllers;
using PakAnas.Models.PakAnas;
using PakAnas.Commons.Models;
using PakAnas.Repositories;
using Toyota.Common.Web.Platform;
using PakAnas.Commons.Constants;
using Toyota.Common.Database;
using PakAnas.Models.PakAnas;
using Toyota.Common.Database;
using PakAnas.Commons.Repositories;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.IO;

namespace PakAnas.Controllers.Repositories
{
    public class PakAnasController : BaseController
    {
        private PakAnasRepository pakanasRepo = PakAnasRepository.Instance;
        private DatabaseManager databaseManager = DatabaseManager.Instance;

        public const string DOWNLOAD_TEMPLATE_FILE_NAME = "DownloadTemplateEmployee.xls";
        public const int DATA_ROW_INDEX_START = 3;

        protected override void Startup()
        {
            Settings.Title = "Master PakAnas";

            int cureentPage = 1;
            int recordPerPage = PagingModel.DEFAULT_RECORD_PER_PAGE;

            PakAnass pakanas = new PakAnass();

            getTypeSizeUpload();

            DoSearch(pakanas, cureentPage, recordPerPage);
        }

        #region GetTypeSizeUpload
        private void getTypeSizeUpload()
        {
            string UploadType =
                pakanasRepo.FindBySettingCd(CommonConstant.SUFE_ORG_INPUT);
            ViewData["UploadType"] = UploadType;

            string x = pakanasRepo.FindBySettingCd(CommonConstant.SUFS_ORG_INPUT);
            double UploadSize = Convert.ToDouble(x);
            ViewData["UploadSize"] = UploadSize;


        }
        #endregion

        #region Search
        public ActionResult Search(PakAnass pakanas, int currentPage, int recordPerPage)
        {
            try
            {
                DoSearch(pakanas, currentPage, recordPerPage);
            }
            catch (Exception ex)
            {
                return Json("Error : " + ex.Message, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_GridView");
        }
        #endregion

        #region DoSearch
        private void DoSearch(PakAnass pakanas, int currentPage, int recordPerPage)
        {
            PagingModel pmodel = new PagingModel(pakanasRepo.SearchCount(pakanas),
                currentPage, recordPerPage);
            ViewData["Paging"] = pmodel;

            IList<PakAnass> listData = pakanasRepo.Search(pakanas, pmodel.Start, pmodel.End);

            ViewData["ListDatapakanas"] = listData;
        }
        #endregion

        #region AddEditSave
        public ActionResult AddEditSave(string screenMode, PakAnass data)
        {
            AjaxResult ajaxResult = new AjaxResult();
            RepoResult repoResult = null;
            IDBContext db = databaseManager.GetContext();

            Toyota.Common.Credential.User u = Lookup.Get<Toyota.Common.Credential.User>();
            string userName = u.Username;

            try
            {

                repoResult = pakanasRepo.InsertUpdate(db, userName, data, screenMode);

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
        #endregion

        #region GetByKey
        public JsonResult GetByKey(long? org_codeId)
        {
            AjaxResult ajaxResult = new AjaxResult();

            PakAnass result = null;

            IDBContext db = databaseManager.GetContext();

            try
            {
                result = pakanasRepo.GetByKey(org_codeId);

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
        #endregion

        #region Delete
        public JsonResult Delete(List<string> selectedKeys)
        {
            AjaxResult ajaxResult = new AjaxResult();
            RepoResult repoResult = null;

            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();
                repoResult = pakanasRepo.DeleteMultiple(db, selectedKeys);
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
        #endregion

        #region DownloadDataToExcel
        public void DownloadFileExcel(PakAnass pakanas, int? PageFlag)
        {

            byte[] result = null;
            string fileName = null;

            try
            {
                if (PageFlag == 0)
                {
                    pakanas.RowsPerPage = Int32.MaxValue;
                }

                IList<PakAnass> data =
                    pakanasRepo.Search(pakanas, pakanas.CurrentPage,
                        pakanas.RowsPerPage);


                fileName = string.Format("PakAanas-{0}.xls",
                        DateTime.Now.ToString("yyyyMMddHHmmss"));
                result = pakanasRepo.GenerateDownloadFile(data);
            }
            catch (Exception e)
            {
                //
            }

            this.SendDataAsAttachment(fileName, result);
        }

        public IList<PakAnass> SearchCommon(PakAnass o, ViewDataDictionary v, string pageModelViewData, string listViewData)
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

            IList<PakAnass> l = db.Fetch<PakAnass>(qList, o);

            return l;
        }

        #endregion

        #region UploadData
        public ActionResult UploadDataFile(HttpPostedFileBase file, string uploadMode)
        {

            AjaxResult ajaxResult = new AjaxResult();
            RepoResult reporesult = null;
            IDBContext db = databaseManager.GetContext();

            IList<string> errMesgs = new List<string>();
            IList<PakAnass> data = new List<PakAnass>();

            Toyota.Common.Credential.User u = Lookup.Get<Toyota.Common.Credential.User>();
            string userName = u.Username;

            try
            {
                data = this.GetDataLocalUploadExcel(file, errMesgs);

                if (errMesgs.Count >= 1)
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] {
                        string.Format("(0)", errMesgs[0])
                };
                }
                else
                {
                    db.BeginTransaction();

                    foreach (PakAnass loopInsert in data)
                    {
                        reporesult = pakanasRepo.InsertUpdate(db, userName, loopInsert, uploadMode);
                    }

                    CopyPropertiesRepoToAjaxResult(reporesult, ajaxResult);

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

        private IList<PakAnass> GetDataLocalUploadExcel(HttpPostedFileBase file,
            IList<string> errMesgs)
        {
            HSSFWorkbook hssfwb = null;

            using (System.IO.Stream file2 = file.InputStream)
            {
                hssfwb = new HSSFWorkbook(file2);
            }

            if (hssfwb == null)
            {
                throw new ArgumentException("Cannot create Workbook object from excel file" + file.FileName);
            }

            IRow row = null;
            ICell cell = null;
            IList<PakAnass> listPakanas = new List<PakAnass>();

            int indexRow = DATA_ROW_INDEX_START;
            bool isAllCellEmpty = true;
            bool isBreak = false;

            ISheet sheet = hssfwb.GetSheetAt(0);
            for (indexRow = DATA_ROW_INDEX_START; indexRow <= sheet.LastRowNum; indexRow++)
            {
                isAllCellEmpty = true;
                isBreak = false;
                row = sheet.GetRow(indexRow);
                row = sheet.GetRow(indexRow);
                if (row != null) //null is when the row only contains empty celss
                {
                    PakAnass Pak = new PakAnass();

                    //org_code
                    try
                    {

                        cell = row.GetCell(0);
                        if (cell == null || cell.CellType == CellType.BLANK)
                        {
                            cell = row.GetCell(1);
                            if (indexRow != DATA_ROW_INDEX_START && (cell == null || cell.CellType == CellType.BLANK))
                            {
                                break;
                            }
                            else
                            {
                                errMesgs.Add(string.Format("org_code row {0} is empty", indexRow + 1));
                                isAllCellEmpty = true;
                            }
                        }
                        else
                        {
                            if (cell.CellType == CellType.NUMERIC)
                            {
                                Pak.org_code = Convert.ToString(cell.NumericCellValue);
                                isAllCellEmpty = false;
                            }
                            else if (cell.CellType == CellType.STRING)
                            {
                                Pak.org_code = cell.StringCellValue;
                                isAllCellEmpty = false;
                            }
                            else
                            {
                                errMesgs.Add(string.Format("org_code row {0} is incorrect Format", indexRow + 1));
                                isAllCellEmpty = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errMesgs.Add(string.Format("Unable To get value of PakAnas No at row {0}, Error Mesg : {1}",
                            indexRow + 1, ex.Message));
                    }

                    //org_name
                    try
                    {
                        cell = row.GetCell(1);
                        if (cell.CellType == CellType.BLANK)
                        {
                            errMesgs.Add(string.Format("org_name row {0} is empty", indexRow + 1));
                            isAllCellEmpty = true;
                        }
                        else
                        {
                            if (cell.CellType == CellType.STRING)
                            {
                                Pak.org_name = cell.StringCellValue;
                                isAllCellEmpty = false;
                            }
                            else
                            {
                                errMesgs.Add(string.Format("org_name row {0} is Incorrect Format", indexRow + 1));
                                isAllCellEmpty = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errMesgs.Add(string.Format("Unable to get value of org_name at row {0}, Error Mesg : {1}",
                            indexRow + 1, ex.Message));
                        isAllCellEmpty = true;
                    }

                    listPakanas.Add(Pak);
                }

              } 
              return listPakanas;
        }
        #endregion

        #region Download Template
        public void DownloadTemplate()
        {
            string fileName = "DownloadTemplatePakAnasToUpload.xls";
            string filesTMP = HttpContext.Request.MapPath("~" +
                CommonConstant.TEMPLATE_EXCEL_DIR + "/" + fileName);
            FileStream ftmp = new FileStream(filesTMP, FileMode.Open, FileAccess.Read);
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

                for (int x = 0; x <= 1; x++)
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
