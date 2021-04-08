using Toyota.Common.Web.Platform;

using PakAnas.Commons.Models;
using Toyota.Common.Credential;
using System.Web.Configuration;
using System.Configuration;
using System.Web;
using System;
using PakAnas.Models;
using PakAnas.Commons.Constants;

namespace PakAnas.Commons.Controllers
{
    public class HelperController : BaseUploadDownloadController
    {
        protected RepoResult LockData(long dataId, string moduleId, string functionId, string lockedBy, CommonConstant.DATA_LOCK_ACTION_TYPE actionType)
        {
            DataLockRepo documentLockRepo = new DataLockRepo(dataId, moduleId, functionId, lockedBy, actionType.ToString());
            return documentLockRepo.LockData();
        }
        protected RepoResult UnlockData(long dataId, string moduleId, string functionId, string lockedBy, CommonConstant.DATA_LOCK_ACTION_TYPE actionType)
        {
            DataLockRepo documentLockRepo = new DataLockRepo(dataId, moduleId, functionId, lockedBy, actionType.ToString());
            return documentLockRepo.ReleaseData();
        }
    }
}