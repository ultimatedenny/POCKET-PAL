using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POCKETPAL_API.Controllers
{
    public class EformController : Controller
    {
        // GET: Eform
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Notif(string deviceId)
        {
            //deviceId = "egIjNQCJQj-qYpusnmO-S8:APA91bG5scLvEuxYpG0YP5XffcPC9gRjZmf4mjif9k5__oIQhogSfqkcKAGTHKoXP5lDNOzj5G1lZlrDEeMVYNPyEKY-HNqUeQduXyDdYmsEEtRy08BgXLz54ca_U3NJx8wbwIQgm4nP";
            //string title = "TEST NOTIF";
            //string body = "MESSAGE";

            string title = "Eform Notification";
            string body = "You have a pending Approval in E-Form";
            var click_action = "";

            TestController NewEform = new TestController();
            return Json(NewEform.TrySend(deviceId, title, body, click_action), JsonRequestBehavior.AllowGet);
        }
    }
}