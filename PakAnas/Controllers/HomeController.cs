using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace CrudSystem.Controllers
{
    public class HomeController1 : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "Dashboard";
        }

        public ActionResult WidgetSettings()
        {
            return PartialView("_WidgetSettings");
        }
    }
}
