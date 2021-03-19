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

namespace PakAnas.Controllers
{
    public class PakAnasController : BaseController
    {
        private PakAnasRepository pakanasRepo = PakAnasRepository.Instance;
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        protected override void Startup()
        {
            Settings.Title = "Master PakAnas";

            int cureentPage = 1;
            int recordPerPage = PagingModel.DEFAULT_RECORD_PER_PAGE;

            PakAnass pakanas = new PakAnass();
            DoSearch(pakanas, cureentPage, recordPerPage);
        }
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

    }
}
