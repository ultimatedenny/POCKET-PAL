using POCKETPAL_API.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using POCKETPAL_API.Models;

namespace POCKETPAL_API.Controllers
{
    public class ExitPermitController : Controller
    {
        ActionExitPermit EP = new ActionExitPermit();
        public ActionResult Index()
        {
            string APIName = WebConfigurationManager.AppSettings["APIName"];
            string APIVersion = WebConfigurationManager.AppSettings["APIVersion"];
            string Final = APIName + " - " + APIVersion;
            return Json(Final, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetExitPermit(string EPNo)
        {
            return Json(EP.GetExitPermit(EPNo), JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateExitPermitOut(string EPNo, string UseId)
        {
            return Json(EP.UpdateExitPermitOut(EPNo, UseId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateExitPermitIn(string EPNo, string UseId)
        {
            return Json(EP.UpdateExitPermitIn(EPNo, UseId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPlantEP()
        {
            return Json(EP.GetPlant(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDeptEP(string Plant)
        {
            return Json(EP.GetDept(Plant), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetApproverEP(string Dept, string Plant)
        {
            return Json(EP.GetApprover(Dept, Plant), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserEP(string UseId)
        { 
            return Json(EP.GetUserEP(UseId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCompanyTransTime()
        {
            return Json(EP.GetCompanyTransTime(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult PostEP(string UseId, string UseDep, string PlantID, string Destination, string Out, string In, string Remarks,
                                 string ActOut, string ActIn, string Date, string CompTrans, string CompTransTime,
                                 string OTH, string Status, string Approver, ModelExitPermitDetail[] EPDetailss)
        {
            

            return Json(EP.PostEP(UseId, UseDep, PlantID, Destination, Out, In, Remarks, ActIn, ActOut, Date, CompTrans, CompTransTime, OTH, Status, Approver, EPDetailss), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetExitPermitAllowBeforeTimeOut()
        {
            return Json(EP.GetExitPermitAllowBeforeTimeOut(), JsonRequestBehavior.AllowGet);
        }
    }
}