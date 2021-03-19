using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Database;
using PakAnas.Commons.Models;
using PakAnas.Commons.Repositories;

namespace PakAnas.Commons.Helpers
{
    public static class BaseRepoHelper
    {
        public static void SearchToView<T>(this T o, ViewDataDictionary v, string pageModelViewData, string listViewData)
        {
            Type t = o.GetType();
            //StringBuilder b = new StringBuilder("");
            int pDot = t.Namespace.LastIndexOf(".");
            int pLen = t.Namespace.Length;

            string dir = (pDot > 1 && (pDot < (pLen - 1))) ? (t.Namespace.Substring(pDot + 1, pLen - pDot - 1) + "/") : "";

            string qCount = dir + t.Name + "_Count";
            string qList = dir + t.Name + "_List";

            //b.AppendFormat("{0}\t{1}\t{2}\tPage={3}\tList={4}\tQCount={5}\tQList={6}",
            //     t.Name, t.Namespace, t.FullName, pageModelViewData, listViewData,
            //     qCount, qList
            //     );

            IDBContext db = BaseRepo.Db;

            long count = db.SingleOrDefault<int>(qCount, o);

            BaseModel m = o as BaseModel;
            PagingModel pmodel = null;
            if (m.RowsPerPage <= 0)
            {
                m.CurrentPage = 1;
                m.RowsPerPage = PagingModel.DEFAULT_RECORD_PER_PAGE;
            }

            pmodel = new PagingModel(count, m.CurrentPage, m.RowsPerPage);
            //PagingModel pmodel = new PagingModel(count, m.CurrentPage, m.RowsPerPage);
            v[pageModelViewData] = pmodel;

            IList<T> l = db.Fetch<T>(qList, m);
            v[listViewData] = l;
        }

        public static void SearchToView<T>(this T o, ViewDataDictionary v, string pageModelViewData, string listViewData, string IdSuffix)
        {
            Type t = o.GetType();
            //StringBuilder b = new StringBuilder("");
            int pDot = t.Namespace.LastIndexOf(".");
            int pLen = t.Namespace.Length;

            string dir = (pDot > 1 && (pDot < (pLen - 1))) ? (t.Namespace.Substring(pDot + 1, pLen - pDot - 1) + "/") : "";

            string qCount = dir + t.Name + "_Count";
            string qList = dir + t.Name + "_List";

            //b.AppendFormat("{0}\t{1}\t{2}\tPage={3}\tList={4}\tQCount={5}\tQList={6}",
            //     t.Name, t.Namespace, t.FullName, pageModelViewData, listViewData,
            //     qCount, qList
            //     );

            IDBContext db = BaseRepo.Db;

            long count = db.SingleOrDefault<int>(qCount, o);

            BaseModel m = o as BaseModel;
            PagingModel pmodel = null;
            if (m.RowsPerPage <= 0)
            {
                m.CurrentPage = 1;
                m.RowsPerPage = PagingModel.DEFAULT_RECORD_PER_PAGE;
            }
            pmodel = new PagingModel(count, m.CurrentPage, m.RowsPerPage);
            pmodel.IdSuffix = IdSuffix;
            //PagingModel pmodel = new PagingModel(count, m.CurrentPage, m.RowsPerPage);
            v[pageModelViewData] = pmodel;

            IList<T> l = db.Fetch<T>(qList, m);
            v[listViewData] = l;
        }

        public static T FindById<T>(this T o)
        {
            Type t = o.GetType();
            //StringBuilder b = new StringBuilder("");
            int pDot = t.Namespace.LastIndexOf(".");
            int pLen = t.Namespace.Length;

            string dir = (pDot > 1 && (pDot < (pLen - 1))) ? (t.Namespace.Substring(pDot + 1, pLen - pDot - 1) + "/") : "";

            string qList = dir + t.Name + "_FindById";

            //b.AppendFormat("{0}\t{1}\t{2}\tPage={3}\tList={4}\tQCount={5}\tQList={6}",
            //     t.Name, t.Namespace, t.FullName, pageModelViewData, listViewData,
            //     qCount, qList
            //     );

            IDBContext db = BaseRepo.Db;

            T res = db.SingleOrDefault<T>(qList, o);
            return res;
        }

        public static IList<T> FindDataByCriteria<T>(this T o)
        {
            Type t = o.GetType();
            //StringBuilder b = new StringBuilder("");
            int pDot = t.Namespace.LastIndexOf(".");
            int pLen = t.Namespace.Length;

            string dir = (pDot > 1 && (pDot < (pLen - 1))) ? (t.Namespace.Substring(pDot + 1, pLen - pDot - 1) + "/") : "";

            string qList = dir + t.Name + "_FindByCriteria";

            //b.AppendFormat("{0}\t{1}\t{2}\tPage={3}\tList={4}\tQCount={5}\tQList={6}",
            //     t.Name, t.Namespace, t.FullName, pageModelViewData, listViewData,
            //     qCount, qList
            //     );

            IDBContext db = BaseRepo.Db;

            return db.Fetch<T>(qList, o);
        }
    }
}