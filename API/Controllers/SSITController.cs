using POCKETPAL_API.Actions;
using POCKETPAL_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POCKETPAL_API.Controllers
{
    public class SSITController : Controller
    {
        // GET: SSIT
        ActionSSIT ssitAct = new ActionSSIT();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetIsUserSSITVerified(string UserId)
        {
            return Json(ssitAct.GetIsUserSSITVerified(UserId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCountDataSSIT_BY_USERGROUP(string UserId, string UserGroup, string ApprCat, string Product, string SSITCode)
        {
            return Json(ssitAct.GetCountDataSSIT_BY_USERGROUP(UserId, UserGroup, ApprCat, Product, SSITCode), JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckScanSSIT(string Type, string UserId, string UserGroup, string ApprCat, string Product, string SSITCode)
        {
            return Json(ssitAct.CheckScanSSIT(Type, UserId, UserGroup, ApprCat, Product, SSITCode), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveScanSSIT(string Type, string UserId, string UserGroup, string ApprCat, string SSITCode)
        {
            return Json(ssitAct.SaveScanSSIT(Type, UserId, UserGroup, ApprCat, SSITCode), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GETPLANT()
        {
            return Json(ssitAct.GETPLANT(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GETVENDOR(string Plant)
        {
            return Json(ssitAct.GETVENDOR(Plant), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetailListSSIT_SCRAP(string Type, string UserId, string UserGroup, string ApprCat, string Product, string SSITCode)
        {
            return Json(ssitAct.DetailListSSIT_SCRAP(Type, UserId, UserGroup, ApprCat, Product, SSITCode), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DetailListSSIT_PR(string Type, string UserId, string UserGroup, string ApprCat, string Product, string SSITCode)
        {
            return Json(ssitAct.DetailListSSIT_PR(Type, UserId, UserGroup, ApprCat, Product, SSITCode), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ApproveSSIT(string Type, string UserId, string UserGroup, string ApprCat, string SSITCode, string SSITCount, string Amount, string AppVersion, string Vendor, string Action)
        {
            return Json(ssitAct.ApproveSSIT(Type, UserId, UserGroup, ApprCat, SSITCode, SSITCount, Amount, AppVersion, Vendor, Action), JsonRequestBehavior.AllowGet);
        }
        public JsonResult RejectSSIT(string Type, string UserId, string UserGroup, string ApprCat, string SSITCode, string SSITCount, string Amount, string AppVersion, string Reason)
        {
            return Json(ssitAct.RejectSSIT(Type, UserId, UserGroup, ApprCat, SSITCode, SSITCount, Amount, AppVersion, Reason), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetailSummarySSIT_SCRAP(string Type, string UserId, string UserGroup, string ApprCat, string Product, string SSITCode)
        {
            return Json(ssitAct.DetailSummarySSIT_SCRAP(Type, UserId, UserGroup, ApprCat, Product, SSITCode), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetailSummarySSIT_PR(string Type, string UserId, string UserGroup, string ApprCat, string Product, string SSITCode)
        {
            return Json(ssitAct.DetailSummarySSIT_PR(Type, UserId, UserGroup, ApprCat, Product, SSITCode), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GETPRODUCT()
        {
            return Json(ssitAct.GETPRODUCT(), JsonRequestBehavior.AllowGet);
        }

    }
}