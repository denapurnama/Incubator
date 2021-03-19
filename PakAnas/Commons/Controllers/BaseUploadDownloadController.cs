using PakAnas.Commons.Models;
using System.Web.Mvc;
using System.Web;
using System;
using System.IO;
using System.Diagnostics;
using System.Web.Script.Serialization;
using PakAnas.Commons.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PakAnas.Commons.Controllers
{
    public class BaseUploadDownloadController : BaseController
    {
        //protected const string APP_SETTING_TEMP_UPLOAD_FILE_PATH = "TempUploadFilePath";
        //protected const string APP_SETTING_UPLOAD_FILE_PATH = "UploadFilePath";

        protected ActionResult UploadFile(HttpPostedFileBase file, string path)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                if (file != null)
                {
                    //string path = ConfigurationManager.AppSettings[APP_SETTING_TEMP_UPLOAD_FILE_PATH];
                    string fileNameOrigin = file.FileName;
                    string fileNameServer = Guid.NewGuid().ToString() + "_" + fileNameOrigin;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string fileNameServerFullPath = Path.Combine(path, fileNameServer);
                    file.SaveAs(fileNameServerFullPath);

                    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                    ajaxResult.Params = new object[] {
                        fileNameOrigin, fileNameServer
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

            return Content(new JavaScriptSerializer().Serialize(ajaxResult), 
                "text/plain", System.Text.Encoding.UTF8);
        }

        // Summary:
        //     Handle Delete File Request
        //
        // Parameters:
        //     fileName : the filename to be download
        //     paths : list path will be search the file (only delete first found file)
        protected ActionResult DeleteFile(string fileName, IList<string> paths)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    DeleteFileProcess(fileName, paths);

                    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                }
                else
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] { "No define the file to be deleted" };
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

        // Summary:
        //     Delete File Process
        //
        // Parameters:
        //     fileName : the filename to be download
        //     paths : list path will be search the file (only delete first found file)
        protected void DeleteFileProcess(string fileName, IList<string> paths)
        {
            string fileNameFullPath = null;
            foreach (string path in paths)
            {
                fileNameFullPath = Path.Combine(path, fileName);

                if (System.IO.File.Exists(fileNameFullPath))
                {
                    break;
                }
            }

            //string path = ConfigurationManager.AppSettings[APP_SETTING_TEMP_UPLOAD_FILE_PATH];
            //string fileNameFullPath = Path.Combine(path, fileName);

            if (System.IO.File.Exists(fileNameFullPath)) 
            {
                System.IO.File.Delete(fileNameFullPath);
            }
        }

        //protected void SendToClientBrowser(string fileName, byte[] hasil)
        //{
        //    Response.Clear();
        //    //Response.ContentType = result.MimeType;
        //    Response.Cache.SetCacheability(HttpCacheability.Private);
        //    Response.Expires = -1;
        //    Response.Buffer = true;

        //    Response.ContentType = "application/octet-stream";
        //    Response.AddHeader("Content-Length", Convert.ToString(hasil.Length));

        //    Response.AddHeader("Content-Disposition", string.Format("{0};FileName=\"{1}\"", "attachment", fileName));
        //    Response.AddHeader("Set-Cookie", "fileDownload=true; path=/");

        //    Response.BinaryWrite(hasil);
        //    Response.End();
        //}

        // Summary:
        //     Handle Download File Request
        //
        // Parameters:
        //     fileName : the filename to be download
        //     paths : list path will be search the file
        //     fileDownloadName : the filename to be send to browser
        protected FileResult DownloadFile(string fileName, IList<string> paths, string fileDownloadName)
        {
            string fileNameServerFullPath = null;
            foreach(string path in paths) {
                fileNameServerFullPath = Path.Combine(path, fileName);

                if (System.IO.File.Exists(fileNameServerFullPath))
                {
                    break;
                }
            }

            if(string.IsNullOrEmpty(fileDownloadName)) 
            {
                fileDownloadName = fileName;
            }

            Response.AddHeader("Set-Cookie", "fileDownload=true; path=/");

            return File(fileNameServerFullPath, MimeExtensionHelper.GetMimeType(fileDownloadName),
                fileDownloadName);/*MimeMapping.GetMimeMapping(fileName)*/
        }

        protected void HousekeepingTempFolder(string path, int remainDays) {
            var directory = new DirectoryInfo(path);
            DateTime limitDt = DateTime.Now.Date.AddDays(-1 * remainDays);

            directory.GetFiles().Where(file => file.LastWriteTime.Date <= limitDt).ToList().ForEach(file => file.Delete());
        }
             
    }
}