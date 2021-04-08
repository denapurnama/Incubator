using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using PakAnas.Commons.Controllers;
using PakAnas.Commons.Models;
using PakAnas.Commons.Repositories;
using PakAnas.Models;
using PakAnas.Models.Items;
using PakAnas.Commons.Constants;
using Item.Repositories;

using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Web;

using System.IO;
using System.Linq;

using System.Diagnostics;
using PakAnas.Commons.Helpers;

namespace PakAnas.Controllers
{
    public class ItemController : BaseController
    {
        private ItemRepository itemRepo = ItemRepository.Instance;
        private DatabaseManager databaseManager = DatabaseManager.Instance;

        public const string DOWNLOAD_TEMPLATE_FILE_NAME = "DownloadTemplateTabelItem.xls";
        public const int DATA_ROW_INDEX_START = 3;

        protected override void Startup()
        {
            Settings.Title = "Menu Item";

            int currentPage = 1;
            int recordPerPage = PagingModel.DEFAULT_RECORD_PER_PAGE;

            Items item = new Items();

            /* getCmboxData();
                                      */
            getTypeSizeUpload();

            DoSearch(item, currentPage, recordPerPage);
        }

        #region GetTypeSizeUpload
        private void getTypeSizeUpload()
        {
            string UploadType =
                itemRepo.FindBySettingCd(CommonConstant.SUFE_EMPLOYEE_INPUT);
            ViewData["UploadType"] = UploadType;

            string UploadSize =
                itemRepo.FindBySettingCd(CommonConstant.SUFS_EMPLOYEE_INPUT);
            ViewData["UploadSize"] = Double.Parse(UploadSize);

            string UploadAttachFileType =
                itemRepo.FindBySettingCd(CommonConstant.SUFE_EMPLOYEE_INPUT_ATTACHMENT);
            ViewData["UploadAttachFileType"] = UploadAttachFileType;

            string UploadAttachFileSize =
                itemRepo.FindBySettingCd(CommonConstant.SUFS_EMPLOYEE_INPUT_ATTACHMENT);
            ViewData["UploadAttachFileSize"] = Double.Parse(UploadAttachFileSize);

        }
        #endregion

        #region Search
        public ActionResult Search(Items item, int currentPage, int recordPerPage)
        {
            try
            {
                DoSearch(item, currentPage, recordPerPage);
            }
            catch (Exception ex)
            {
                return Json("Error : " + ex.Message, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_GridView");
        }
        #endregion

        #region DoSearch
        private void DoSearch(Items item, int currentPage, int recordPerPage)
        {
            PagingModel pmodel = new PagingModel(itemRepo.SearchCount(item),
                currentPage, recordPerPage);
            ViewData["Paging"] = pmodel;

            IList<Items> listData = itemRepo.Search(item,
                pmodel.Start, pmodel.End);

            ViewData["listDataAdaptive"] = listData;

        }
        #endregion

        #region AddEditSave
        public AjaxResult Validation(Items data)
        {
            AjaxResult ajaxResult = new AjaxResult();

            ajaxResult.ErrMesgs = new string[1];
            ajaxResult.Result = AjaxResult.VALUE_ERROR;

            if (data.ITEM_CODE == null || data.ITEM_CODE == "")
            {
                ajaxResult.ErrMesgs[0] = "ERROR Controller: Item code should not be empty";
            }
            else if (data.ITEM_NAME == null || data.ITEM_NAME == "")
            {
                ajaxResult.ErrMesgs[0] = "ERROR Controller: Item Name should not be empty";
            }

            else if (data.ITEM_UM == null || data.ITEM_UM == "")
            {
                ajaxResult.ErrMesgs[0] = "ERROR Controller: Item Um should not be empty";
            }
            else
            {
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
            }

            return ajaxResult;
        }

        public ActionResult AddEditSave(string screenMode, Items data)
        {
            AjaxResult ajaxResult = new AjaxResult();
            RepoResult repoResult = null;
            IDBContext db = databaseManager.GetContext();

            Toyota.Common.Credential.User u = Lookup.Get<Toyota.Common.Credential.User>();
            string userName = u.Username;

            try
            {
                db.BeginTransaction();

                ajaxResult = Validation(data); //Validasi Controller

                if (AjaxResult.VALUE_SUCCESS.Equals(ajaxResult.Result))
                {
                    if (CommonConstant.SCREEN_MODE_EDIT.Equals(screenMode))
                    {
                        Items dtUpload = itemRepo.GetByKey(data.ITEM_CODE);

                        if (dtUpload.UPLOAD_PATH != null)
                        {
                            if (dtUpload.UPLOAD_PATH != data.UPLOAD_HALF_PATH)
                            {
                                this.DeleteUploadedFile(dtUpload.UPLOAD_PATH);
                            }
                        }
                    }

                    repoResult = itemRepo.InsertUpdate(db, userName, data, screenMode);

                    CopyPropertiesRepoToAjaxResult(repoResult, ajaxResult);

                    if (AjaxResult.VALUE_ERROR.Equals(ajaxResult.Result))
                    {
                        db.AbortTransaction();
                    }
                    else
                    {
                        db.CommitTransaction();

                        string sourceDirPath =
                            itemRepo.FindBySettingCd(CommonConstant.SYS_FILE_LOC_TEMP_FOLDER);

                        string destDirPath =
                            itemRepo.FindBySettingCd(CommonConstant.SYS_FILE_LOC_MAIN_FOLDER);

                        MoveFiles(sourceDirPath, destDirPath, data.UPLOAD_HALF_PATH);
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

                repoResult = itemRepo.DeleteMultiple(db, selectedKeys);

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
        
   public JsonResult DeleteMultiple(List<string> selectedKeys)
   {
       AjaxResult ajaxResult = new AjaxResult();
       RepoResult repoResult = null;

       IDBContext db = databaseManager.GetContext();

       try
       {
           db.BeginTransaction();

           repoResult = itemRepo.DeleteMultiple(db, selectedKeys);

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
        public JsonResult GetByKey(string itemscode)
        {
            AjaxResult ajaxResult = new AjaxResult();

            Items result = null;

            IDBContext db = databaseManager.GetContext();

            try
            {
                result = itemRepo.GetByKey(itemscode);

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

        #region DownloadDataToExcel
        public void DownloadFileExcel(Items item, int? PageFlag)
        {
            byte[] result = null;
            string fileName = null;

            try
            {
                if (PageFlag == 0)
                {
                    item.RowsPerPage = Int32.MaxValue;
                }

                IList<Items> data =
                    itemRepo.Search(item, item.CurrentPage,
                        item.RowsPerPage);

                fileName = string.Format("Item-{0}.xls",
                        DateTime.Now.ToString("yyyyMMddHHmmss"));
                result = itemRepo.GenerateDownloadFile(data);
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
            string fileName = "DownloadTemplateItemToUpload.xls";
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

        #region UploadData
        public ActionResult UploadDataFile(HttpPostedFileBase file, string uploadMode)
        {

            AjaxResult ajaxResult = new AjaxResult();
            RepoResult repoResult = null;
            IDBContext db = databaseManager.GetContext();

            IList<string> errMesgs = new List<string>();
            IList<Items> data = new List<Items>();

            Toyota.Common.Credential.User u = Lookup.Get<Toyota.Common.Credential.User>();
            string userName = u.Username;

            try
            {
                data = this.GetDataLocalUploadExcel(file, errMesgs);

                if (errMesgs.Count >= 1)
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] { 
                        string.Format("{0}", errMesgs[0])
                    };
                }
                else
                {
                    db.BeginTransaction();

                    foreach (Items loopInsert in data)
                    {
                        repoResult = itemRepo.InsertUpdate(db, userName, loopInsert, uploadMode);
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

        private IList<Items> GetDataLocalUploadExcel(HttpPostedFileBase file,
            IList<string> errMesgs)
        {
            HSSFWorkbook hssfwb = null;

            using (System.IO.Stream file2 = file.InputStream)
            {
                hssfwb = new HSSFWorkbook(file2);
            }

            if (hssfwb == null)
            {
                throw new ArgumentNullException("Cannot create Workbook object from excel file " + file.FileName);
            }

            IRow row = null;
            ICell cell = null;
            IList<Items> listItem = new List<Items>();

            int indexRow = DATA_ROW_INDEX_START;
            bool isAllCellEmpty = true;
            bool isBreak = false;

            ISheet sheet = hssfwb.GetSheetAt(0);
            for (indexRow = DATA_ROW_INDEX_START; indexRow <= sheet.LastRowNum; indexRow++)
            {
                isAllCellEmpty = true;
                isBreak = false;
                row = sheet.GetRow(indexRow);
                if (row != null) //null is when the row only contains empty cells 
                {
                    Items item = new Items();

                    //Item Code
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
                                errMesgs.Add(string.Format("Item Code row {0} is empty", indexRow + 1));
                                isAllCellEmpty = true;
                            }
                        }
                        else
                        {
                            if (cell.CellType == CellType.STRING)
                            {
                                item.ITEM_CODE = cell.StringCellValue;
                                isAllCellEmpty = false;
                            }
                            else if (cell.CellType == CellType.STRING)
                            {
                                item.ITEM_NAME = cell.StringCellValue;
                                isAllCellEmpty = false;
                            }
                            else if (cell.CellType == CellType.STRING)
                            {
                                item.ITEM_UM = cell.StringCellValue;
                                isAllCellEmpty = false;
                            }
                            else
                            {
                                errMesgs.Add(string.Format("Item Code row {0} is Incorrect Format", indexRow + 1));
                                isAllCellEmpty = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errMesgs.Add(string.Format("Unable to get value of Employee No at row {0}, Error Mesg : {1}",
                            indexRow + 1, ex.Message));
                        isAllCellEmpty = true;
                    }

                    //Item Name
                    try
                    {
                        cell = row.GetCell(1);
                        if (cell.CellType == CellType.BLANK)
                        {
                            errMesgs.Add(string.Format("Item Name row {0} is empty", indexRow + 1));
                            isAllCellEmpty = true;
                        }
                        else
                        {
                            if (cell.CellType == CellType.STRING)
                            {
                                item.ITEM_NAME = cell.StringCellValue;
                                isAllCellEmpty = false;

                            }
                            else
                            {
                                errMesgs.Add(string.Format("Item Name row {0} is Incorrect Format", indexRow + 1));
                                isAllCellEmpty = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errMesgs.Add(string.Format("Unable to get value of Employee Name at row {0}, Error Mesg : {1}",
                            indexRow + 1, ex.Message));
                        isAllCellEmpty = true;

                    }



                    //Item Um
                    try
                    {
                        cell = row.GetCell(7);
                        if (cell.CellType == CellType.BLANK)
                        {
                            errMesgs.Add(string.Format("Item um row {0} is empty", indexRow + 1));
                            isAllCellEmpty = true;
                        }
                        else
                        {
                            if (cell.CellType == CellType.STRING)
                            {
                                item.ITEM_UM = cell.StringCellValue;
                                isAllCellEmpty = false;
                            }
                            else
                            {
                                errMesgs.Add(string.Format("Item um row {0} is Incorrect Format", indexRow + 1));
                                isAllCellEmpty = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errMesgs.Add(string.Format("Unable to get value of Address at row {0}, Error Mesg : {1}",
                            indexRow + 1, ex.Message));
                        isAllCellEmpty = true;

                    }

                    if (isBreak || isAllCellEmpty) break;

                    listItem.Add(item);

                }
            }

            return listItem;
        }
        #endregion

        #region Upload download Attachment
        public FileResult DownloadFile(string fileName, string fileNameHalfPath)
        {
            return DownloadFileAttachment(fileName, fileNameHalfPath);
        }

        protected FileResult DownloadFileAttachment(string fileName, string fileNameHalfPath)
        {
            string fileDownloadName = null;

            var tempF = fileNameHalfPath.Split('\\');
            int countF = tempF.Length;
            fileDownloadName = tempF[countF - 1];
            fileNameHalfPath = fileNameHalfPath.Replace(@"\\", @"\");
            fileNameHalfPath = fileNameHalfPath.Remove(0, 1);

            string prePathMain = null;
            string prePathTemp = null;
            string fileNameServerFullPath = null;

            prePathMain = itemRepo.FindBySettingCd(CommonConstant.SYS_FILE_LOC_MAIN_FOLDER);
            prePathTemp = itemRepo.FindBySettingCd(CommonConstant.SYS_FILE_LOC_TEMP_FOLDER);

            string mainFullPath = Path.Combine(prePathMain, fileNameHalfPath);
            string tempFullPath = Path.Combine(prePathTemp, fileNameHalfPath);

            if (System.IO.File.Exists(mainFullPath))
            {
                fileNameServerFullPath = mainFullPath;
            }
            else
            {
                fileNameServerFullPath = tempFullPath;
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                fileDownloadName = fileName;
            }

            Response.AddHeader("Set-Cookie", "fileDownload=true; path=/");

            return File(fileNameServerFullPath, MimeExtensionHelper.GetMimeType(fileDownloadName),
                fileDownloadName);
        }

        public ActionResult UploadAttachment(HttpPostedFileBase file, string uploadItemCode, string uploadMode)
        {
            Toyota.Common.Credential.User u = Lookup.Get<Toyota.Common.Credential.User>();
            uploadItemCode = u.Username;

            string folderName = "Photo";

            return UploadFileMultiplePhysicalPath(file, folderName, uploadItemCode, uploadMode);
        }

        protected ActionResult UploadFileMultiplePhysicalPath(HttpPostedFileBase file, string folderName, string id, string mode)
        {
            AjaxResult ajaxResult = new AjaxResult();
            RepoResult repoResult = null;

            try
            {
                if (file != null)
                {
                    //string path = ConfigurationManager.AppSettings[APP_SETTING_TEMP_UPLOAD_FILE_PATH];
                    string path = null;
                    string tempPath = null;
                    string halfPath = null;
                    //path = Server.MapPath(smRepo.FindBySystemTypeCd(CommonConstant.SYS_FILE_LOC_TEMP_FOLDER).SYSTEM_VALUE_TXT);
                    //tempPath = Server.MapPath(smRepo.FindBySystemTypeCd(CommonConstant.SYS_FILE_LOC_TEMP_FOLDER).SYSTEM_VALUE_TXT);
                    halfPath = "\\" + folderName;
                    path = itemRepo.FindBySettingCd(CommonConstant.SYS_FILE_LOC_TEMP_FOLDER);
                    path = path + halfPath;
                    tempPath = path;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    HousekeepingTempFolder(path, 1);

                    string fileNameOrigin = file.FileName;
                    string fileNameServer = Guid.NewGuid().ToString() + "_" + fileNameOrigin;

                    string fileNameServerFullPath = Path.Combine(path, fileNameServer);
                    string fileNameHalfPath = Path.Combine(halfPath, fileNameServer);

                    file.SaveAs(fileNameServerFullPath);

                    tempPath = tempPath + "" + fileNameServer;
                    fileNameHalfPath = fileNameHalfPath.Replace(@"\", @"\\");

                    repoResult = itemRepo.InsertAttachment(id, path, fileNameServer);

                    CopyPropertiesRepoToAjaxResult(repoResult, ajaxResult);

                    ajaxResult.Params = new object[] {
                        fileNameOrigin, fileNameServer, fileNameServerFullPath, id, tempPath, fileNameHalfPath, mode
                    };
                }
                else
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] { "No file uploaded, please reupload again the file" };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] { 
                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                };
            }

            return Json(ajaxResult);
        }

        protected void DeleteUploadedFile(string dataPath)
        {
            string prePathMain = itemRepo.FindBySettingCd(CommonConstant.SYS_FILE_LOC_MAIN_FOLDER);

            dataPath = dataPath.Replace(@"\\", @"\");
            dataPath = dataPath.Remove(0, 1);

            string path = Path.Combine(prePathMain, dataPath);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        protected bool MoveFiles(string sourceDirPath, string destDirPath, string fileNameServer)
        {
            bool isSuccess = true;
            try
            {
                if (!string.IsNullOrEmpty(fileNameServer))
                {
                    if (!Directory.Exists(destDirPath))
                    {
                        Directory.CreateDirectory(destDirPath);
                    }

                    itemRepo.DeleteAttachment(fileNameServer);

                    System.IO.File.Copy(sourceDirPath + fileNameServer, destDirPath + fileNameServer, true);
                    System.IO.File.Delete(sourceDirPath + fileNameServer);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                isSuccess = false;

            }
            return isSuccess;
        }

        protected void HousekeepingTempFolder(string path, int remainDays)
        {
            var directory = new DirectoryInfo(path);
            DateTime limitDt = DateTime.Now.Date.AddDays(-1 * remainDays);

            directory.GetFiles().Where(file => file.LastWriteTime.Date <= limitDt).ToList().ForEach(file => file.Delete());
        }
        #endregion

    }
}
