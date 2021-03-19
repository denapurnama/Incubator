using Toyota.Common.Web.Platform;

using PakAnas.Commons.Models;
using Toyota.Common.Credential;
using System.Web.Configuration;
using System.Configuration;
using System.Web;
using System;

namespace PakAnas.Commons.Controllers
{
    public class BaseController : PageController
    {
        protected User GetLoginUser()
        {
            return Lookup.Get<User>();
        }

        protected string GetLoginUserId()
        {
            return GetLoginUser().Username;
        }

        protected void CopyPropertiesRepoToAjaxResult(RepoResult source, AjaxResult dest)
        {
            if (source == null)
            {
                dest = null;
                return;
            }

            if (dest != null && source != null)
            {
                dest.Result = source.Result;
                dest.ProcessId = source.ProcessId;
                dest.SuccMesgs = source.SuccMesgs;
                dest.ErrMesgs = source.ErrMesgs;
                dest.Params = source.Params;
            }
        }

        protected void SendDataAsAttachment(string fileName, byte[] data)
        {
            Response.Clear();
            //Response.ContentType = result.MimeType;
            Response.Cache.SetCacheability(HttpCacheability.Private);
            Response.Expires = -1;
            Response.Buffer = true;

            Response.ContentType = "application/octet-stream";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.ContentType = "application/vnd.ms-excel";

            Response.AddHeader("Content-Length", Convert.ToString(data.Length));

            Response.AddHeader("Content-Disposition", string.Format("{0};FileName=\"{1}\"", "attachment", fileName));
            Response.AddHeader("Set-Cookie", "fileDownload=true; path=/");

            Response.BinaryWrite(data);
            Response.End();
        }

        // Summary:
        //     Retrieves Max request length
        //
        // Returns:
        //     Max request length in KiloBytes
        //     Can be set on web.config :
        //      <httpRuntime maxRequestLength="10240"/> in KB
        //
        protected int? GetMaxRequestLength()
        {
            int? maxRequestLength = null;
            HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            if (section != null)
            {
                maxRequestLength = section.MaxRequestLength;
            }

            return maxRequestLength;
        }

        // Summary:
        //     Retrieves Max request length
        //
        // Returns:
        //     Max request length in Bytes
        //     Can be set on web.config :
        //      <httpRuntime maxRequestLength="10240"/> in KB
        //
        protected int? GetMaxRequestLengthInBytes()
        {
            int? maxRequestLength = GetMaxRequestLength();

            return maxRequestLength != null ? maxRequestLength * 1024 : null;
        }
    }
}