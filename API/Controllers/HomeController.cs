using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace POCKETPAL_API.Controllers
{
    public class HomeController : Controller
    {
        ILog log = LogManager.GetLogger(typeof(HomeController));
        public ActionResult Index()
        {
            log.Info("API STARTED....");
            string APIName = WebConfigurationManager.AppSettings["APIName"];
            string APIVersion = WebConfigurationManager.AppSettings["APIVersion"];
            string Final = APIName + " - " + APIVersion;
            return Json(Final, JsonRequestBehavior.AllowGet);
        }
    }
}